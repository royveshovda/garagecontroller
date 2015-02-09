import socket
import datetime
import sys
import signal
from time import sleep

from kombu import Connection, Queue
from kombu.mixins import ConsumerMixin
from kombu.pools import producers
import pifacedigitalio as pi_face

from Settings import get_settings
from Parser import parse


def start_receiving(filename):
    # TODO: Send error to separate channel
    # TODO: Log to file in case of missing communication
    settings = get_settings(filename)
    connection_string = settings["RabbitMqConnectionString"]
    queue_name = settings["RabbitMqDeviceQueueName"]
    device_id = settings["DeviceId"]
    exchange = settings["RabbitMqDeviceResponseExchangeName"]
    heartbeat_interval_in_seconds = int(settings["HeartbeatIntervalInSeconds"])

    queue = Queue(queue_name)

    with Connection(connection_string, heartbeat=20) as conn:
        reconnect(conn)
        with conn.Consumer(queue, callbacks=[process_message]) as consumer:
            with producers[conn].acquire(block=True) as producer:
                running = True
                print("Running\n")
                heartbeat = datetime.datetime.utcnow()
                consumer.consume()
                while running:
                    reconnect(conn)
                    temp_heartbeat = datetime.datetime.utcnow()
                    if (temp_heartbeat - heartbeat).total_seconds() > heartbeat_interval_in_seconds:
                        heartbeat = temp_heartbeat
                        send_heartbeat(producer, device_id, exchange)
                    try:
                        conn.heartbeat_check()
                        conn.drain_events(timeout=9)
                    except KeyboardInterrupt:
                        running = False
                    except socket.timeout:
                        running = True
                    except ConnectionResetError:
                        reconnect(conn)
                        running = True


def reconnect(local_connection):
    if not local_connection.connected:
        local_connection.connect()
        local_connection.ensure_connection()


def send_heartbeat(producer, device_id, exchange):
    message = "Heartbeat: " + datetime.datetime.utcnow().isoformat()
    producer.publish(message, exchange=exchange, routing_key=device_id)


def process_message(body, message):
    # noinspection PyBroadException
    try:
        session_id, door, created, expiry, signature = parse(body)
        toggle_door(door)
        message.ack()
    except Exception as err:
        print("Error processing message: " + body)
        print("Error: {0}".format(err))


def toggle_door(door):
    try:
        integer_door = int(door)
    except ValueError:
        print("Door " + door + " is not supported in this system")
        return

    if integer_door == 1 or integer_door == 2:
        integer_door -= 1
        print("Door: " + str(door))
        toggle_door_pi_face(integer_door)
    else:
        print("Door " + door + " is not supported in this system")


def toggle_door_pi_face(door):
    pi_face.init()
    pi_face.digital_write(door, 1)
    sleep(1)
    pi_face.digital_write(door, 0)


def set_exit_handler(func):
    signal.signal(signal.SIGTERM, func)


# noinspection PyUnusedLocal
def on_exit(sig, func=None):
    print("exit handler triggered")
    sys.exit(1)


class Worker(ConsumerMixin):
    def __init__(self, connection, queue):
        self.connection = connection
        self.queue = queue
        self.connect_max_retries = 10

    def get_consumers(self, consumer, channel):
        cons = consumer(self.queue, callbacks=[process_message], accept=['json'])
        cons.qos()
        return [
            cons,
        ]


if __name__ == '__main__':
    set_exit_handler(on_exit)
    fileName = sys.argv[1]
    start_receiving(fileName)