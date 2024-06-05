using Microsoft.EntityFrameworkCore;
using ReportApi.Enums;
using ReportApi.Models.Entities;

namespace ReportApi.Models
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Report>()
                .HasMany(r => r.ReportDetails)
                .WithOne(d => d.Report)
                .HasForeignKey(d => d.ReportId);

            modelBuilder.Entity<ReportDetail>(entity =>
            {
                entity.Property(e => e.LastIndex).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Voltage).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Power).HasColumnType("decimal(18,2)");
            });

            // Test data for Reports
            var reports = new List<Report>
            {
                new Report
                {
                    Id = Guid.NewGuid(),
                    MeterId = Guid.NewGuid(),
                    SerialNumber = "SN123456",
                    Name = "Report 1",
                    RequestDate = DateTime.Now.AddDays(-10),
                    CreationDate = DateTime.Now.AddDays(-9),
                    Status = EReportStatus.Completed
                },
                new Report
                {
                    Id = Guid.NewGuid(),
                    MeterId = Guid.NewGuid(),
                    SerialNumber = "SN654321",
                    Name = "Report 2",
                    RequestDate = DateTime.Now.AddDays(-5),
                    CreationDate = DateTime.Now.AddDays(-4),
                    Status = EReportStatus.InProgress
                },
                new Report
                {
                    Id = Guid.NewGuid(),
                    MeterId = Guid.NewGuid(),
                    SerialNumber = "SN112233",
                    Name = "Report 3",
                    RequestDate = DateTime.Now.AddDays(-2),
                    CreationDate = null,
                    Status = EReportStatus.FailedToGenerate
                }
            };

            modelBuilder.Entity<Report>().HasData(reports);

            // Test data for ReportDetails
            var reportDetails = new List<ReportDetail>
            {
                new ReportDetail
                {
                    Id = Guid.NewGuid(),
                    ReportId = reports[0].Id,
                    ReadingTime = DateTime.Now.AddDays(-10).AddHours(1),
                    Power = 123.45m,
                    Voltage = 220.5m,
                    LastIndex = 1000.1m
                },
                new ReportDetail
                {
                    Id = Guid.NewGuid(),
                    ReportId = reports[1].Id,
                    ReadingTime = DateTime.Now.AddDays(-5).AddHours(2),
                    Power = 150.67m,
                    Voltage = 230.0m,
                    LastIndex = 1050.2m
                },
                new ReportDetail
                {
                    Id = Guid.NewGuid(),
                    ReportId = reports[2].Id,
                    ReadingTime = DateTime.Now.AddDays(-2).AddHours(3),
                    Power = 175.89m,
                    Voltage = 240.7m,
                    LastIndex = 1100.3m
                }
            };

            modelBuilder.Entity<ReportDetail>().HasData(reportDetails);


        }

    }
}
