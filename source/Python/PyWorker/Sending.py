import pika
from kombu import Connection, Exchange

from Settings import get_settings

def main(fileName):
    settings = get_settings(fileName)
    connection_string = settings["RabbitMqConnectionString"]
    exchange_name = settings["RabbitMqCommandExchangeName"]

    exchange = Exchange(exchange_name)

    with Connection(connection_string) as conn:
        producer = conn.Producer()
        producer.publish("Hello from within python3", exchange=exchange)



if __name__ == '__main__':
    fileName = "D:\Settings.yaml"
    main(fileName)