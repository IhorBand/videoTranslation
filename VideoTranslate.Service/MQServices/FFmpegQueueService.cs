using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using VideoTranslate.Shared.Abstractions.Services.MQServices;
using VideoTranslate.Shared.DTO.Configuration;
using VideoTranslate.Shared.DTO.MQModels;

namespace VideoTranslate.Service.MQServices
{
    public class FFmpegQueueService : IFFmpegQueueService
    {
        private readonly string hostname;
        private readonly string password;
        private readonly string queueName = "ffmpeg_convert_for_recognition";
        private readonly string username;
        private IConnection connection;

        public FFmpegQueueService(RabbitMQConfiguration rabbitMQConfiguration)
        {
            this.hostname = rabbitMQConfiguration.HostName;
            this.username = rabbitMQConfiguration.User;
            this.password = rabbitMQConfiguration.Password;

            this.CreateConnection();
        }

        public void SendConvertVideoRecognizeCommand(ConvertVideoRecognizeCommand convertVideoRecognizeCommand)
        {
            if (this.ConnectionExists())
            {
                using (var channel = this.connection.CreateModel())
                {
                    channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(convertVideoRecognizeCommand);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: string.Empty, routingKey: this.queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = this.hostname,
                    UserName = this.username,
                    Password = this.password
                };
                this.connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (this.connection != null)
            {
                return true;
            }

            this.CreateConnection();

            return this.connection != null;
        }
    }
}
