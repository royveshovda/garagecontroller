import pika
from Settings import get_settings


def start_receiving(filename):
    settings = get_settings(filename)
    host = settings["RabbitMqHost"]
    username = settings["RabbitMqUsername"]
    password = settings["RabbitMqPassword"]
    queue = settings["RabbitMqCommandQueueName"]
    credentials = pika.PlainCredentials(username, password)
    parameters = pika.ConnectionParameters(credentials=credentials,
                                           host=host)
    connection = pika.BlockingConnection(parameters)
    channel = connection.channel()

    print(" [*] Waiting for messages. To exit press CTRL+C")
    channel.basic_consume(callback, queue=queue)
    try:
        channel.start_consuming()
    except KeyboardInterrupt:
        channel.stop_comsuming()

    connection.close()


def callback(ch, method, properties, body):
    print(" [x] Received %r", body)


if __name__ == '__main__':
    fileName = "D:\Settings.yaml"
    start_receiving(fileName)