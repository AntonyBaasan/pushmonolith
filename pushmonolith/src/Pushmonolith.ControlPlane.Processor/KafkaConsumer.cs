using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushmonolith.ControlPlane.Processor
{
    public  class KafkaConsumer
    {
        public void Consume(string topic)
        {
            configuration["group.id"] = "csharp-group-1";
            configuration["auto.offset.reset"] = "earliest";

            // creates a new consumer instance 
            using (var consumer = new ConsumerBuilder<string, string>(configuration.AsEnumerable()).Build())
            {
                consumer.Subscribe(topic);
                while (true)
                {
                    // consumes messages from the subscribed topic and prints them to the console
                    var cr = consumer.Consume();
                    Console.WriteLine($"Consumed event from topic {topic}: key = {cr.Message.Key,-10} value = {cr.Message.Value}");
                }

                // closes the consumer connection
                consumer.Close();
            }
        }   
       
    }
}
