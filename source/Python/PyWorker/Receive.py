from Settings import get_settings
from kombu import Connection, Queue
from Parser import parse

def start_receiving(filename):
    settings = get_settings(filename)
    connection_string = settings["RabbitMqConnectionString"]
    queue_name = settings["RabbitMqCommandQueueName"]

    queue = Queue(queue_name)

    with Connection(connection_string) as conn:
        with conn.Consumer(queue, callbacks=[process_message]) as consumer:
            running = True
            while running:
                try:
                    conn.drain_events()
                except KeyboardInterrupt:
                    running = False


def process_message(body, message):
    session_id, door, created, expiry, signature = parse(body)
    message.ack()
    print("SessionId: " + session_id)
    print("Door: " + door)
    print("Created: " + created)
    print("Expiry: " + expiry)
    print("Signature: " + signature)


if __name__ == '__main__':
    fileName = "/Users/royveshovda/src/Settings.yaml"
    start_receiving(fileName)