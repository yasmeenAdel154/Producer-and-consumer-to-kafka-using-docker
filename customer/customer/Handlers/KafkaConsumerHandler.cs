using Confluent.Kafka;
using customer.Data;
using customer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace customer.Handlers
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly ApplicationDbContext _context;

        public KafkaConsumerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly string topic = "offer";
       

        public Task StartAsync(CancellationToken cancellationToken)
        {

            
            var conf = new ConsumerConfig
            {
                GroupId = "sa",
                BootstrapServers = "kafka:29092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);

                        Offer offer = JsonConvert.DeserializeObject<Offer>(consumer.Message.Value);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                        
                        if ( offer.deleted ) {
                            DeleteOffer(offer.offerID);
                        }
                        else
                        {
                            PostOffer(offer) ;
                        }

                        Console.WriteLine(offer.deleted);
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }

        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        

        public async Task<string> PostOffer(Offer offer)
        {
            if (_context.offer == null)
            {
                return ("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            
            _context.offer.Add(offer);
            await _context.SaveChangesAsync();

            return ("Added");
        }

        public async Task<string> DeleteOffer(int id)
        {
            if (_context.offer == null)
            {
                return "NotFound";
            }
            var offer = await _context.offer.FindAsync(id);
            if (offer == null)
            {
                return "NotFound";
            }

            _context.offer.Remove(offer);
            await _context.SaveChangesAsync();

            return "Deleted";
        }


        public async Task<string> PutOffer(int id, Offer offer)
        {
            if (id != offer.offerID)
            {
                return "Bad request";
            }

            _context.Entry(offer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferExists(id))
                {
                    return "NotFound";
                }
                else
                {
                    throw;
                }
            }

            return "NotFound";
        }

        private bool OfferExists(int id)
        {
            return (_context.offer?.Any(e => e.offerID == id)).GetValueOrDefault();
        }
    }
}
