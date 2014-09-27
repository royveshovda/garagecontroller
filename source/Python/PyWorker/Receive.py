import sys, signal
from Settings import get_settings
from kombu import Connection, Queue
from Parser import parse
import pifacedigitalio as pi_face
from time import sleep


# TODO: Send error to separate channel
# TODO: Log to file in case of missing communication
# TODO: Send heartbeat on regular basis

def start_receiving(filename):
    settings = get_settings(filename)
    connection_string = settings["RabbitMqConnectionString"]
    queue_name = settings["RabbitMqCommandQueueName"]

    queue = Queue(queue_name)

    with Connection(connection_string) as conn:
        with conn.Consumer(queue, callbacks=[process_message]) as consumer:
            running = True
            print("Running\n")
            while running:
                try:
                    conn.drain_events()
                except KeyboardInterrupt:
                    running = False


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


def on_exit(sig, func=None):
    print("exit handler triggered")
    sys.exit(1)


if __name__ == '__main__':
    set_exit_handler(on_exit)
    fileName = sys.argv[1]
    # fileName = "/Users/royveshovda/src/Settings.yaml"
    start_receiving(fileName)