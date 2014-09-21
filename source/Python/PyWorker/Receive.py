import sys
from Settings import get_settings
from kombu import Connection, Queue
from Parser import parse
import pifacedigitalio as piface
from time import sleep


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
        idoor = int(door)
    except ValueError:
        print("Door " + door + " is not supported in this system")
        return

    if idoor == 1 or idoor == 2:
        idoor -= 1
        piface.init()
        print("Door: " + str(door))
        piface.digital_write(idoor, 1)
        sleep(1)
        piface.digital_write(idoor, 0)
    else:
        print("Door " + door + " is not supported in this system")


if __name__ == '__main__':
    fileName = sys.argv[1]
    # fileName = "/Users/royveshovda/src/Settings.yaml"
    start_receiving(fileName)