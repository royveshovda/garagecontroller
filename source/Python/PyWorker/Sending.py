import pika
from Settings import get_settings

def main(fileName):
    settings = get_settings(fileName)
    host = settings["RabbitMqHost"]
    username = settings["RabbitMqUsername"]
    password = settings["RabbitMqPassword"]
    exchange = settings["RabbitMqCommandExchangeName"]
    credentials = pika.PlainCredentials(username, password)
    parameters = pika.ConnectionParameters(credentials=credentials,
                                           host=host)
    connection = pika.BlockingConnection(parameters)
    channel = connection.channel()

    channel.basic_publish(exchange=exchange,
                          routing_key='',
                          body='Hello World, again!')

    connection.close()



if __name__ == '__main__':
    fileName = "D:\Settings.yaml"
    main(fileName)