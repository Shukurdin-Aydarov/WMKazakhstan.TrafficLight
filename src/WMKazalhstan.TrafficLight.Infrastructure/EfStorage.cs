using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using WMKazakhstan.TrafficLight.Core;
using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Infrastructure.Entities;

using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.Infrastructure.DataStorage
{
    public class EfStorage : IStorage
    {
        private readonly Context context;

        public EfStorage(Context context)
        {
            this.context = context;
        }

        public async Task ClearAsync()
        {
            using (var transaction = context.Database.BeginTransaction()) 
            {
                try
                {
                    context.Database.ExecuteSqlRaw("DELETE FROM Sequences");
                    context.Database.ExecuteSqlRaw("DELETE FROM Observations");

                    await transaction.CommitAsync();
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                    
                    throw;
                }
            }
        }

        public async Task<Guid> CreateSequenceAsync()
        {
            var sequence = await context.Sequences.AddAsync(new SequenceEntity());

            await context.SaveChangesAsync();

            return sequence.Entity.Id;
        }

        public async ValueTask<IEnumerable<Observation>> FindObservationAsync(Guid sequenceId)
        {
            var sequence = await context.Sequences
                .Include(s => s.Observations)
                .FirstOrDefaultAsync(s => s.Id == sequenceId);

            if (sequence == null)
                Throws.SequenceNotFound();

            return sequence.Observations
                .OrderBy(o => o.Date)
                .Select(o =>
                {
                    var trafficLight = new Light(
                    o.Color,
                    new TrafficLightDigits(o.High, o.Low));

                    var predictedDigits = o
                        .Digits
                        .Select(x => new TrafficLightDigits(x))
                        .ToArray();

                    return new Observation(sequenceId, trafficLight, predictedDigits);
                });
        }

        public async Task SaveObservationAsync(Guid sequenceId, Light trafficLight, IEnumerable<TrafficLightDigits> digits)
        {
            var sequence = await context.Sequences.FindAsync(sequenceId);

            if(sequence == null)
                Throws.SequenceNotFound();

            var observation = new ObservationEntity
            {
                Date = DateTimeOffset.UtcNow,
                Color = trafficLight.Color,
                High = trafficLight.Digits.High.ToBitString(),
                Low = trafficLight.Digits.Low.ToBitString(),
                Digits = digits.Select(x => x.ToNumber()).ToArray()
            };

            sequence.Observations.Add(observation);

            await context.SaveChangesAsync();
        }
    }
}
