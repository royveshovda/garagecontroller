from Settings import get_settings
from kombu import Connection, Queue

def start_receiving(filename):
    settings = get_settings(filename)
    connection_string = settings["RabbitMqConnectionString"]
    queue_name = settings["RabbitMqCommandQueueName"]

    queue = Queue(queue_name)

    with Connection(connection_string) as conn:
        with conn.Consumer(queue, callbacks=[process_message]) as consumer:
            while True:
                conn.drain_events()




#    try:
#        channel.start_consuming()
#    except KeyboardInterrupt:
#        channel.stop_comsuming()


def process_message(body, message):
    print(body)
    message.ack()


if __name__ == '__main__':
    fileName = "D:\Settings.yaml"
    start_receiving(fileName)