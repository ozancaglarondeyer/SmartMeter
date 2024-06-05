using MeterApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterApi.Models
{
    public class MeterDbContext : DbContext
    {
        public MeterDbContext(DbContextOptions<MeterDbContext> options) : base(options)
        {

        }

        public DbSet<Meter> Meters { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var meterId1 = Guid.NewGuid();
            var meterId2 = Guid.NewGuid();

            modelBuilder.Entity<Meter>()
                .HasMany(m => m.MeterReadings)
                .WithOne(mr => mr.Meter)
                .HasForeignKey(mr => mr.MeterId);

            // Test data for Meters
            var meters = new List<Meter>
            {
                new Meter
                {
                    Id = Guid.NewGuid(),
                    SerialNumber = "Meter123456",
                },
                new Meter
                {
                    Id = Guid.NewGuid(),
                    SerialNumber = "Meter654321",
                },
                new Meter
                {
                    Id = Guid.NewGuid(),
                    SerialNumber = "Meter112233",
                }
            };

            modelBuilder.Entity<Meter>().HasData(meters);

            // Test data for MeterReadings
            var meterReadings = new List<MeterReading>
            {
                new MeterReading
                {
                    Id = Guid.NewGuid(),
                    MeterId = meters[0].Id,
                    ReadingTime = DateTime.Now.AddDays(-10).AddHours(1),
                    Voltage = 220.5,
                    LastIndex = 1000.1,
                    Power = 123.45
                },
                new MeterReading
                {
                    Id = Guid.NewGuid(),
                    MeterId = meters[1].Id,
                    ReadingTime = DateTime.Now.AddDays(-5).AddHours(2),
                    Voltage = 230.0,
                    LastIndex = 1050.2,
                    Power = 150.67
                },
                new MeterReading
                {
                    Id = Guid.NewGuid(),
                    MeterId = meters[2].Id,
                    ReadingTime = DateTime.Now.AddDays(-2).AddHours(3),
                    Voltage = 240.7,
                    LastIndex = 1100.3,
                    Power = 175.89
                }
            };

            modelBuilder.Entity<MeterReading>().HasData(meterReadings);
        }


    }
}
