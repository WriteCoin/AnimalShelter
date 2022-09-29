using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;

namespace AnimalShelter
{
    public partial class animal_shelterContext : DbContext
    {
        public animal_shelterContext()
        {
        }

        public animal_shelterContext(DbContextOptions<animal_shelterContext> options)
            : base(options)
        {
        }

        public static DbContextOptions<animal_shelterContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<animal_shelterContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;

            return options;
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Animal> Animals { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<RequestStatus> RequestStatuses { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Shelter> Shelters { get; set; } = null!;
        public virtual DbSet<ShelterAnimal> ShelterAnimals { get; set; } = null!;
        public virtual DbSet<ShelterWorkHour> ShelterWorkHours { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WeekDay> WeekDays { get; set; } = null!;
        public virtual DbSet<WorkHour> WorkHours { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(64)
                    .HasColumnName("city");

                entity.Property(e => e.FullAddr)
                    .HasMaxLength(255)
                    .HasColumnName("full_addr");

                entity.Property(e => e.House).HasColumnName("house");

                entity.Property(e => e.Street)
                    .HasMaxLength(64)
                    .HasColumnName("street");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("address_to_users");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admins");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("admins_to_person");
            });

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.ToTable("animals");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Breed)
                    .HasMaxLength(100)
                    .HasColumnName("breed");

                entity.Property(e => e.Color)
                    .HasMaxLength(100)
                    .HasColumnName("color");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .HasColumnName("type");

                entity.Property(e => e.Vaccinated).HasColumnName("vaccinated");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate).HasColumnName("birth_date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Fio)
                    .HasMaxLength(255)
                    .HasColumnName("fio");

                entity.Property(e => e.RegDate).HasColumnName("reg_date");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(20)
                    .HasColumnName("telephone");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("requests");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(255)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactTelephone)
                    .HasMaxLength(20)
                    .HasColumnName("contact_telephone");

                entity.Property(e => e.RegDate).HasColumnName("reg_date");

                entity.Property(e => e.RequestStatusId).HasColumnName("request_status_id");

                entity.Property(e => e.RequestTypeId).HasColumnName("request_type_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("requests_to_address");

                entity.HasOne(d => d.RequestStatus)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("requests_to_request_statuses");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("requests_to_request_types");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("requests_to_users");
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.ToTable("request_statuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("request_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Shelter>(entity =>
            {
                entity.ToTable("shelters");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(32)
                    .HasColumnName("telephone");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Shelters)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelters_to_address");
            });

            modelBuilder.Entity<ShelterAnimal>(entity =>
            {
                entity.ToTable("shelter_animals");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdoptionDate).HasColumnName("adoption_date");

                entity.Property(e => e.AnimalId).HasColumnName("animal_id");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.ShelterId).HasColumnName("shelter_id");

                entity.Property(e => e.TermShelter).HasColumnName("term_shelter");

                entity.HasOne(d => d.Animal)
                    .WithMany(p => p.ShelterAnimals)
                    .HasForeignKey(d => d.AnimalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelter_animals_to_animals");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.ShelterAnimals)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelter_animals_to_requests");

                entity.HasOne(d => d.Shelter)
                    .WithMany(p => p.ShelterAnimals)
                    .HasForeignKey(d => d.ShelterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelter_animals_to_shelters");
            });

            modelBuilder.Entity<ShelterWorkHour>(entity =>
            {
                entity.ToTable("shelter_work_hours");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ShelterId).HasColumnName("shelter_id");

                entity.Property(e => e.WorkHoursId).HasColumnName("work_hours_id");

                entity.HasOne(d => d.Shelter)
                    .WithMany(p => p.ShelterWorkHours)
                    .HasForeignKey(d => d.ShelterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelter_work_hours_to_shelters");

                entity.HasOne(d => d.WorkHours)
                    .WithMany(p => p.ShelterWorkHours)
                    .HasForeignKey(d => d.WorkHoursId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shelter_work_hours_to_work_hours");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_to_person");
            });

            modelBuilder.Entity<WeekDay>(entity =>
            {
                entity.ToTable("week_days");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<WorkHour>(entity =>
            {
                entity.ToTable("work_hours");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.HourMax).HasColumnName("hour_max");

                entity.Property(e => e.HourMin).HasColumnName("hour_min");

                entity.Property(e => e.MinuteMax).HasColumnName("minute_max");

                entity.Property(e => e.MinuteMin).HasColumnName("minute_min");

                entity.Property(e => e.WeekDayId).HasColumnName("week_day_id");

                entity.HasOne(d => d.WeekDay)
                    .WithMany(p => p.WorkHours)
                    .HasForeignKey(d => d.WeekDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("work_hours_to_week_days");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
