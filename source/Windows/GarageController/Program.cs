using System;
using System.IO;
using GarageController.Properties;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

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

            var filename = Settings.Default.ConfigFilename;
            var config = GetConfig(filename);

            //TODO: use heartbeat interval from config
            //TODO: Use SSL if set in config
            var worker = new QueueWorker(config.RabbitMqHost, config.RabbitMqUsername, config.RabbitPassword, config.RabbitMqQueueName, messageHandler.HandleCommand, log);
            worker.Start();

            Console.Clear();

            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();

            worker.Stop();
        }

        private static Config GetConfig(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException();

            var raw = File.ReadAllText(filename);
            var config = JsonConvert.DeserializeObject<Config>(raw);

            return config;
        }
    }
}
