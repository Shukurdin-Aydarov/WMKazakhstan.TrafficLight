using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Infrastructure.Entities;

namespace WMKazakhstan.TrafficLight.Infrastructure
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        internal DbSet<SequenceEntity> Sequences { get; set; }
        internal DbSet<ObservationEntity> Observations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObservationEntity>()
                .Property(e => e.Digits)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(";", StringSplitOptions.RemoveEmptyEntries)
                          .Select(val => int.Parse(val))
                          .ToArray()
                );

            modelBuilder.Entity<ObservationEntity>()
                .Property(e => e.Color)
                .HasConversion(new EnumToStringConverter<Color>());
        }
    }
}
