import pika


def main():
    credentials = pika.PlainCredentials('tester', 'GoGoTester91')
    parameters = pika.ConnectionParameters(credentials=credentials,
                                           host="rv-broker.cloudapp.net")
    connection = pika.BlockingConnection(parameters)
    channel = connection.channel()

    channel.basic_publish(exchange='',
                          routing_key='Testing',
                          body='Hello World!')

    connection.close()



if __name__ == '__main__':
    main()