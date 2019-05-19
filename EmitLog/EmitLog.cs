using RabbitMQ.Client;
using System;
using System.Text;
using System.Timers;

namespace EmitLog
{
    class EmitLog
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SetTimer();

            Console.ReadKey();
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            var messageTimer = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            messageTimer.Elapsed += OnTimedEvent;
            messageTimer.AutoReset = true;
            messageTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.15.33",
                UserName = "renatocolaco",
                Password = "secnet123"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");


                var message = GetMessage();
                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }

            Console.WriteLine("A new message published to the queue at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        private static string GetMessage()
        {
            return $"Hello World! {DateTime.Now:HH:mm:ss}";
        }
    }
}
