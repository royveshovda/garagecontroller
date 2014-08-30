namespace GarageController
{
    public class Config
    {
        public string RabbitMqHost { get; set; }
        public bool RabbitMqUseSsl { get; set; }
        public string RabbitMqUsername { get; set; }
        public string RabbitPassword { get; set; }
        public ushort RabbitHearBeatIntervalInSeconds { get; set; }
        public string RabbitMqQueueName { get; set; }
    }
}
