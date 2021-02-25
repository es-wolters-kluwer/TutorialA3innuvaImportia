namespace a3innuva.Tutorial.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using a3innuva.Tutorial.Implementations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.Extensions.Configuration;

    public class AppDbContext : DbContext
    {
        public DbSet<UserDataEntity> UserData { get; set; }

        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDataEntityConfiguration());

            List<UserDataEntity> entities = new List<UserDataEntity>();

            int code = 1000;
            Random rd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int temp = code + i;
                int group = rd.Next(1, 9)*1000;

                entities.Add(new UserDataEntity()
                {
                    Code = temp.ToString(), 
                    Group = group.ToString(),
                    Id = Guid.NewGuid(), 
                    Name = temp.ToString(), 
                    PostalCode = i.ToString().PadLeft(5,'0'),
                    VatNumber = group.ToString().StartsWith("4") ? "53604200D" : string.Empty,
                    Letter = "A-"
                });

            }

            builder.Entity<UserDataEntity>().HasData(
                entities
            );


            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=a3innuvaTutorial;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }


    public class UserDataEntityConfiguration : IEntityTypeConfiguration<UserDataEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
