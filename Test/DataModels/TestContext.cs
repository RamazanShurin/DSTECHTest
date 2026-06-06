using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Test.DataModels
{
    public class TestContext : DbContext
    {
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<TypeEquipment> TypeEquipments { get; set; }
        public DbSet<Application> Applications { get; set; }

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = WebApplication.CreateBuilder();
            string connString = builder.Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ApplicationStatus applicationStatus = new ApplicationStatus { Id = 1, Name = "New" };
            ApplicationStatus applicationStatus2 = new ApplicationStatus { Id = 2, Name = "InProgress" };
            ApplicationStatus applicationStatus3 = new ApplicationStatus { Id = 3, Name = "Approved" };
            ApplicationStatus applicationStatus4 = new ApplicationStatus { Id = 4, Name = "Rejected" };
            ApplicationStatus applicationStatus5 = new ApplicationStatus { Id = 5, Name = "Cancelled" };

            TypeEquipment typeEquipment = new TypeEquipment { Id = 1, Name = "Ноутбук" };
            TypeEquipment typeEquipment2 = new TypeEquipment { Id = 2, Name = "Монитор" };
            TypeEquipment typeEquipment3 = new TypeEquipment { Id = 3, Name = "Принтер" };
            TypeEquipment typeEquipment4 = new TypeEquipment { Id = 4, Name = "Клавиатура" };
            TypeEquipment typeEquipment5 = new TypeEquipment { Id = 5, Name = "Мышь" };

            modelBuilder.Entity<ApplicationStatus>().HasData(new ApplicationStatus[] { applicationStatus, applicationStatus2, applicationStatus3, applicationStatus4, applicationStatus5 });
            modelBuilder.Entity<TypeEquipment>().HasData(new TypeEquipment[] { typeEquipment, typeEquipment2, typeEquipment3, typeEquipment4, typeEquipment5 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
