using EStockMarketCompanyService.Message.Send.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EStockMarketCompanyService.Message.Send
{
    public class DeleteStockRabbitMqService : IDeleteStockRabbitMqService
    {
        private IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly string _connectionUri;
        private readonly string _queueName;

        public DeleteStockRabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;

            var configSection = _configuration.GetSection("RabbitMq");

            _connectionUri = configSection.GetSection("ConnectionUri").Value;
            _queueName = configSection.GetSection("QueueName").Value;
            
            CreateConnection();
        }

        public void SendDeleteCompany(string deleteCompanyCode)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    var json = JsonConvert.SerializeObject(deleteCompanyCode);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_connectionUri)
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
