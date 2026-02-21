using Microsoft.EntityFrameworkCore;
using MessApplication.models;

namespace MessApplication
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<MealWindow> MealWindows { get; set; }
        public DbSet<MealPricing> MealPricings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<MonthlyBill> MonthlyBills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.QrCodeValue)
                .IsUnique();
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.UserId, a.MealTypeId, a.AttendanceDate })
                .IsUnique();

            //seed data for MealTypes
            modelBuilder.Entity<MealType>().HasData(
                new MealType { Id = 1, Name = "Breakfast", IsActive = true },
                new MealType { Id = 2, Name = "Lunch", IsActive = true },
                new MealType { Id = 3, Name = "Dinner", IsActive = true }
            );

            modelBuilder.Entity<MealWindow>().HasData(
                new MealWindow
                {
                    Id = 1,
                    MealTypeId = 1,
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new MealWindow
                {
                    Id = 2,
                    MealTypeId = 2,
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new MealWindow
                {
                    Id = 3,
                    MealTypeId = 3,
                    StartTime = new TimeSpan(19, 0, 0),
                    EndTime = new TimeSpan(22, 0, 0),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
