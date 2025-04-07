using Microsoft.EntityFrameworkCore;
using OutfitPlaner_Applcation.Models;

namespace OutfitPlaner_Applcation.Data
{
    public class WardrobeDbContext : DbContext
    {
        public WardrobeDbContext(DbContextOptions<WardrobeDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Clothing> Clothing { get; set; }
        public DbSet<ClothingCapsule> ClothingCapsules { get; set; }
        public DbSet<ClothingLook> ClothingLooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(u => u.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.UserName)
                    .HasColumnName("User_Name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Email)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.PasswordHash)
                    .HasColumnName("Password_Hash")
                    .IsRequired();

                entity.Property(u => u.CreatedAt)
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Конфигурация для Clothing
            modelBuilder.Entity<Clothing>(entity =>
            {
                entity.ToTable("Clothing");

                entity.Property(c => c.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.IdUser)
                    .HasColumnName("ID_User");

                entity.Property(c => c.Type)
                    .HasColumnName("Type")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.ImageUrl)
                    .HasColumnName("Image_URL")
                    .IsRequired();

                entity.Property(c => c.Color)
                    .HasColumnName("Color")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Style)
                    .HasColumnName("Style")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Material)
                    .HasColumnName("Material")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Season)
                    .HasColumnName("Season")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(c => c.Condition)
                    .HasColumnName("Condition");

                entity.Property(c => c.AddedAt)
                    .HasColumnName("Added_At")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Настройка связи с User
                entity.HasOne(c => c.IdUserNavigation)
                    .WithMany(u => u.Clothing)
                    .HasForeignKey(c => c.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация для ClothingCapsule (если нужно)
            modelBuilder.Entity<ClothingCapsule>(entity =>
            {
                // Добавьте конфигурацию здесь
            });

            // Конфигурация для ClothingLook (если нужно)
            modelBuilder.Entity<ClothingLook>(entity =>
            {
                // Добавьте конфигурацию здесь
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Data/OutfitPlanner.db");

#if DEBUG
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
#endif
            }
        }
    }
}