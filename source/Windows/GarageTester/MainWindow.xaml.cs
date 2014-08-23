using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using GarageController;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace GarageTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private string _hostName;
        private string _username;
        private string _password;
        private string _exchangeName;
        private string _routingKey;

        private ushort _heartbeatInterval = 30;

        public MainWindow()
        {
            const string filename = "D:\\Config\\GarageController\\testapp.config.json";
            var config = GetConfig(filename);

            _hostName = config.RabbitMqHost;
            _username = config.RabbitMqUsername;
            _password = config.RabbitPassword;
            _heartbeatInterval = config.RabbitHearBeatIntervalInSeconds;
            _exchangeName = config.RabbitMqExchangeName;
            _routingKey = config.RabbitMqRoutingKey;

            InitializeComponent();

            InitializeRabbitMq();


        }

        private static Config GetConfig(string filename)
        {
            if(!File.Exists(filename)) throw new FileNotFoundException();

            var raw = File.ReadAllText(filename);
            var config = JsonConvert.DeserializeObject<Config>(raw);

            return config;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Disconnect();
            base.OnClosing(e);
        }

        private void InitializeRabbitMq()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password,
                RequestedHeartbeat = _heartbeatInterval,
                //Ssl = ssl
            };

        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new ToggleDoorCommand
            {
                DoorNumber = 1,
                Created = DateTime.Now.Ticks,
                Expiry = DateTime.Now.AddSeconds(30).Ticks,
                SessionId = Guid.NewGuid().ToString()
            };

            SendCommand(cmd);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new ToggleDoorCommand
            {
                DoorNumber = 2,
                Created = DateTime.Now.Ticks,
                Expiry = DateTime.Now.AddSeconds(30).Ticks,
                SessionId = Guid.NewGuid().ToString()
            };

            SendCommand(cmd);
        }

        public void SendCommand(ToggleDoorCommand command)
        {
            if (!IsConnected())
            {
                Connect();
            }
            if (IsConnected())
            {
                var messageBody = SerializeToggleDoorCommand(command);
                var messageProperties = GetMessageProperties(command.DoorNumber, command.GetType().Name);
                _model.BasicPublish(_exchangeName, _routingKey, messageProperties, messageBody);
            }
        }

        private static byte[] SerializeToggleDoorCommand(ToggleDoorCommand command)
        {
            string proto = ProtoBuf.Serializer.GetProto<ToggleDoorCommand>();
            byte[] serialized;
            using (var mso = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(mso, command);
                mso.Position = 0;
                serialized = mso.ToArray();
            }
            return serialized;
        }

        private IBasicProperties GetMessageProperties(int doorNumber, string dataType)
        {
            IBasicProperties properties;
            lock (_model)
            {
                properties = _model.CreateBasicProperties();
            }
            properties.SetPersistent(true);
            properties.Headers = new System.Collections.Generic.Dictionary<string, object>
            {
                {"type", dataType},
                {"door", doorNumber}
            };
            return properties;
        }

        public bool IsConnected()
        {
            if (_connectionFactory == null) return false;
            if (_connection == null) return false;
            if (_model == null) return false;
            return _model.IsOpen;
        }

        private bool Connect()
        {
            //var ssl = new SslOption();
            //ssl.Enabled = true;
            //ssl.ServerName = _hostName;

            _connectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password,
                RequestedHeartbeat = _heartbeatInterval,
                //Ssl = ssl
            };

            try
            {
                _connection = _connectionFactory.CreateConnection();

                if (_connection != null && _connection.IsOpen)
                {
                    _model = _connection.CreateModel();
                    return _model != null && _model.IsOpen;
                }
            }
            catch (Exception)
            { }
            return false;  // Failed to create connection
        }

        public void Disconnect()
        {
            if (_model != null)
            {
                lock (_model)
                {
                    _model.Close(200, "Goodbye");
                    _model.Dispose();
                }
                _model = null;
            }

            if (_connection != null)
            {
                lock (_connection)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
                _connection = null;
            }

            if (_connectionFactory != null)
            {
                lock (_connectionFactory)
                {
                    _connectionFactory = null;
                }
            }
        }
    }
}
