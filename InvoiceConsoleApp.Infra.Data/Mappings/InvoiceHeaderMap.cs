using InvoiceConsoleApp.Infra.Data.Extentions;
using InvoiceConsoleApp.Infra.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceConsoleApp.Infra.Data.Mappings
{
    public class InvoiceHeaderMap : EntityTypeConfiguration<InvoiceHeader>
    {
        public override void Map(EntityTypeBuilder<InvoiceHeader> builder)
        {
            builder.ToTable("InvoiceHeader");

            builder.HasKey(t => t.InvoiceId);

            builder.Property(c => c.InvoiceId)
                .HasColumnName("InvoiceId");

            builder.Property(c => c.InvoiceTotal)
                .HasColumnType("float");
        }
    }
}
