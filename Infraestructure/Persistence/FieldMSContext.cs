using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class FieldMSContext : DbContext
    {
        public DbSet<Availability> Availabilities { get; set; }



        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }

        public FieldMSContext(DbContextOptions<FieldMSContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\NahuelCejas;Database=FieldMS_DB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Availability>(entity =>
            {
                entity.ToTable("Availability");
                entity.HasKey(a => a.AvailabilityID);
                entity.Property(a => a.AvailabilityID)
                .ValueGeneratedOnAdd();

                entity.HasOne(a => a.FieldNavigator).WithMany(f => f.Availabilities).HasForeignKey(a => a.FieldID);

            });



            modelBuilder.Entity<Field>(entity =>
            {
                entity.ToTable("Field");
                entity.HasKey(f => f.FieldID);

                entity.Property(f => f.Name)
                    .HasColumnType("varchar(255)")
                    .IsRequired();

                entity.Property(f => f.Size)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(f => f.IsActive)
                    .IsRequired();

                entity.HasOne(f => f.FieldTypeNavigator)
                    .WithMany(ft => ft.Fields)
                    .HasForeignKey(f => f.FieldTypeID);
            });

            modelBuilder.Entity<FieldType>(entity =>
            {
                entity.ToTable("FieldType");
                entity.HasKey(ft => ft.FieldTypeID); //static

                entity.Property(ft => ft.Description)
                    .HasColumnType("nvarchar(50)")
                    .IsRequired();
            });
            PreloadedData.Preload(modelBuilder);
        }

    }
}
