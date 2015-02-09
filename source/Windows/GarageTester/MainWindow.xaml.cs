using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using GarageController;
using GarageTester.Properties;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace GarageTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private readonly Config _config;

        public MainWindow()
        {
            var filename = Settings.Default.ConfigFilename;
            _config = GetConfig(filename);
            InitializeComponent();
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

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new ToggleDoorCommand
            {
                DoorNumber = 1,
                Created = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Expiry = DateTime.Now.AddSeconds(30).ToString(CultureInfo.InvariantCulture),
                SessionId = Guid.NewGuid().ToString(),
                Signature = string.Empty
            };

            SendCommand(cmd);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new ToggleDoorCommand
            {
                DoorNumber = 2,
                Created = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Expiry = DateTime.Now.AddSeconds(30).ToString(CultureInfo.InvariantCulture),
                SessionId = Guid.NewGuid().ToString(),
                Signature = string.Empty
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
                _model.BasicPublish(_config.RabbitMqExchangeName, _config.RabbitMqRoutingKey, messageProperties, messageBody);
            }
        }

        //private static byte[] SerializeToggleDoorCommand(ToggleDoorCommand command)
        //{
        //    byte[] serialized;
        //    using (var mso = new MemoryStream())
        //    {
        //        ProtoBuf.Serializer.Serialize(mso, command);
        //        mso.Position = 0;
        //        serialized = mso.ToArray();
        //    }
        //    return serialized;
        //}

        private static byte[] SerializeToggleDoorCommand(ToggleDoorCommand command)
        {
            var text = JsonConvert.SerializeObject(command);
            return System.Text.Encoding.UTF8.GetBytes(text);
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

// ReSharper disable once UnusedMethodReturnValue.Local
        private bool Connect()
        {
            if (_config.RabbitMqUseSsl)
            {
                var ssl = new SslOption {Enabled = true, ServerName = _config.RabbitMqHost};
                _connectionFactory = new ConnectionFactory
                {
                    HostName = _config.RabbitMqHost,
                    UserName = _config.RabbitMqUsername,
                    Password = _config.RabbitPassword,
                    RequestedHeartbeat = _config.RabbitHearBeatIntervalInSeconds,
                    Ssl = ssl
                };
            }
            else
            {
                _connectionFactory = new ConnectionFactory
                {
                    HostName = _config.RabbitMqHost,
                    UserName = _config.RabbitMqUsername,
                    Password = _config.RabbitPassword,
                    RequestedHeartbeat = _config.RabbitHearBeatIntervalInSeconds,
                };
            }

            try
            {
                _connection = _connectionFactory.CreateConnection();

                if (_connection != null && _connection.IsOpen)
                {
                    _model = _connection.CreateModel();
                    return _model != null && _model.IsOpen;
                }
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (Exception error)
            {
                MessageBox.Show("Error connecting to RabbitMQ: " + error.Message);
            }
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
