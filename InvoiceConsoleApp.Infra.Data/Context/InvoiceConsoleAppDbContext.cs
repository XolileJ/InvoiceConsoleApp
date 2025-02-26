using InvoiceConsoleApp.Infra.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InvoiceConsoleApp.Infra.Data.Context
{
    public class InvoiceConsoleAppDbContext(DbContextOptions<InvoiceConsoleAppDbContext> options) : DbContext(options)
    {
        public DbSet<InvoiceHeader> InvoiceHeader { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InvoiceLine>()
                .HasOne(il => il.InvoiceHeader)
                .WithMany(ih => ih.InvoiceLines)
                .HasForeignKey(il => il.InvoiceNumber)
                .HasPrincipalKey(ih => ih.InvoiceNumber);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

            var connectionString = config["ConnectionStrings:InvoiceConsoleAppConnection"];

            optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;;Database=INVOICE;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True", builder =>
            {
                builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(5), null);
            });

            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}