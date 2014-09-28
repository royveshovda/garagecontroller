import socket
import datetime
import sys
import signal
from time import sleep

from kombu import Connection, Queue
from kombu.mixins import ConsumerMixin
from kombu.pools import producers
#import pifacedigitalio as pi_face

from Settings import get_settings
from Parser import parse


def start_receiving(filename):
    # TODO: Send error to separate channel
    # TODO: Log to file in case of missing communication
    settings = get_settings(filename)
    connection_string = settings["RabbitMqConnectionString"]
    queue_name = settings["RabbitMqDeviceQueueName"]
    device_id = settings["DeviceId"]

    queue = Queue(queue_name)

    with Connection(connection_string, heartbeat=20) as conn:
        reconnect(conn)
        worker = Worker(conn, queue)
        try:
            print("Running")
            worker.run()
        except KeyboardInterrupt:
            print("Exiting")


        # with conn.Consumer(queue, callbacks=[process_message]) as consumer:
        #     with producers[conn].acquire(block=True) as producer:
        #         running = True
        #         print("Running\n")
        #         heartbeat = datetime.datetime.utcnow()
        #         consumer.consume()
        #         while running:
        #             reconnect(conn)
        #             temp_heartbeat = datetime.datetime.utcnow()
        #             if (temp_heartbeat - heartbeat).total_seconds() > 30:
        #                 heartbeat = temp_heartbeat
        #                 send_heartbeat(producer, device_id)
        #             try:
        #                 conn.drain_events(timeout=9)
        #             except KeyboardInterrupt:
        #                 running = False
        #             except socket.timeout:
        #                 #conn.heartbeat_check()
        #                 running = True


def reconnect(local_connection):
    if not local_connection.connected:
        local_connection.connect()

def send_heartbeat(producer, device_id):
    message = "Heartbeat: " + datetime.datetime.utcnow().isoformat()
    producer.publish(message, exchange="GarageKorvettveien7_X", routing_key=device_id)


def process_message(body, message):
    try:
        session_id, door, created, expiry, signature = parse(body)
        toggle_door(door)
        message.ack()
    except:
        print("Error processing message: " + body)


def toggle_door(door):
    try:
        integer_door = int(door)
    except ValueError:
        print("Door " + door + " is not supported in this system")
        return

    if integer_door == 1 or integer_door == 2:
        integer_door -= 1
        print("Door: " + str(door))
        #toggle_door_pi_face(integer_door)
    else:
        print("Door " + door + " is not supported in this system")


#def toggle_door_pi_face(door):
#    pi_face.init()
#    pi_face.digital_write(door, 1)
#    sleep(1)
#    pi_face.digital_write(door, 0)


def set_exit_handler(func):
    signal.signal(signal.SIGTERM, func)


def on_exit(sig, func=None):
    print("exit handler triggered")
    sys.exit(1)


class Worker(ConsumerMixin):
    def __init__(self, connection, queue):
        self.connection = connection
        self.queue = queue

    def get_consumers(self, Consumer, channel):
        return [
            Consumer(self.queue, callbacks=[process_message], accept=['json']),
        ]


if __name__ == '__main__':
    set_exit_handler(on_exit)
    fileName = sys.argv[1]
    # fileName = "/Users/royveshovda/src/Settings.yaml"
    start_receiving(fileName)