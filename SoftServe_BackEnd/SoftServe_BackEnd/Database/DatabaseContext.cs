using Microsoft.EntityFrameworkCore;
using Npgsql;
using SoftServe_BackEnd.Models;

#nullable disable

namespace SoftServe_BackEnd.Database
{
    public class DatabaseContext: DbContext
    {

        static DatabaseContext() => NpgsqlConnection.GlobalTypeMapper.MapEnum<TypeOfVolunteer>("type_of_volunteer");

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=arsen_postgres;Server=localhost;Port=2000;Database=SSDatabase;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum(null, "type_of_volunteer",
                    new[]
                    {
                        "eco", "zoo", "phone", "intelectual", "school", "homeless", "families", "inclusive", "culture",
                        "medecine"
                    })
                .HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("client_pkey");

                entity.ToTable("client");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .HasColumnName("city");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.IsOrganization)
                    .HasColumnName("is_organization");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Site)
                    .HasMaxLength(50)
                    .HasColumnName("site");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("created_by");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("place");
                
                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsRequired()
                    .HasColumnName("type");
                
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("event_createdby_fkey");
            });
        }
    }
}