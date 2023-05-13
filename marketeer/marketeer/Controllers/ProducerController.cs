using Confluent.Kafka;
using marketeer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace marketeer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly ProducerConfig config = new ProducerConfig
        { 
            BootstrapServers = "kafka:29092" 
        };
        private readonly string topic = "offer2";
        [HttpPost]
        public IActionResult Post([FromBody] Offer offer)
        {
            string message = JsonSerializer.Serialize(offer);
            return Created(string.Empty, SendToKafka(topic, message));
        }
        private Object SendToKafka(string topic, string message)
        {
            using (var producer =
                 new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
                        .GetAwaiter()
                        .GetResult();
                    Console.WriteLine(result.Value);
                    //Console.WriteLine(result);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
            return null;
        }
    }
}
