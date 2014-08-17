using System;
using log4net;
using log4net.Config;

namespace GarageController
{
    //TODO: Implement:
    //Connect to RabbitMQ
    //Reconnect if needed

    //Wait for packages:
    //  TogglePort1
    //  TogglePort2
    
    //When received:
    //  Send correct signals
    //  Send response that command was executed

    //On regular basis: Send event signaling heartbeat


    //Parameters:
    // RabbiqMQ: Address
    // RabbiqMQ: UN : garage
    // RabbiqMQ: PW : GoGoGarage64
    // RabbiqMQ: Exchange to send heartbeat to
    // RabbiqMQ: Queue to subscribe to
    // Interval to send heartbeat

    class Program
    {
        static void Main(string[] args)
        {
            var log = LogManager.GetLogger(typeof(Program));
            XmlConfigurator.Configure();

            var messageHandler = new MessageHandler();

            var rabbitMqHostname = Properties.Settings.Default.RabbitMqHostname;
            var rabbitMqUsername = Properties.Settings.Default.RabbitMqUsername;
            var rabbitMqPassword = Properties.Settings.Default.RabbitMqPassword;
            var rabbitMqQueueName = Properties.Settings.Default.RabbitMqQueue;
            var worker = new QueueWorker(rabbitMqHostname, rabbitMqUsername, rabbitMqPassword, rabbitMqQueueName, messageHandler.HandleCommand, log);
            worker.Start();

            Console.Clear();

            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();

            worker.Stop();


            //bool quit = false;


            //while (!quit)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Toggle Door 1: <1>");
            //    Console.WriteLine("Toggle Door 2: <2>");
            //    Console.WriteLine();
            //    Console.WriteLine("End: <q>");
            //    var key = Console.ReadKey(true);

            //    switch (key.Key)
            //    {
            //        case ConsoleKey.Q:
            //            quit = true;
            //            break;
            //        case ConsoleKey.D1:
            //        case ConsoleKey.NumPad1:
            //            controller.ToggleDoor1();
            //            break;
            //        case ConsoleKey.D2:
            //        case ConsoleKey.NumPad2:
            //            controller.ToggleDoor2();
            //            break;
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
        }
    }
}
