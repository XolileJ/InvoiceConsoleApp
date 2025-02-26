using InvoiceConsoleApp.Infra.Data.Extentions;
using InvoiceConsoleApp.Infra.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PPECB.UserLinking.Infra.Data.Mappings
{
    public class InvoiceLineMap : EntityTypeConfiguration<InvoiceLine>
    {
        public override void Map(EntityTypeBuilder<InvoiceLine> builder)
        {
            builder.ToTable("InvoiceLines");

            builder.HasKey(t => t.LineId);

            builder.Property(c => c.LineId)
                .HasColumnName("LineId");

            builder.Property(c => c.UnitSellingPriceExVAT)
                .HasColumnType("float");
        }
    }
}
