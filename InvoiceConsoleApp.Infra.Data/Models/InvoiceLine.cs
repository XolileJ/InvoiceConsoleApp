using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceConsoleApp.Infra.Data.Models
{
    public class InvoiceLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineId { get; set; }

        [Required]
        [StringLength(50)]
        public required string InvoiceNumber { get; set; }


        [StringLength(100)]
        public string? Description { get; set; }

        public float? Quantity { get; set; }

        public float? UnitSellingPriceExVAT { get; set; }

        [ForeignKey("InvoiceNumber")]
        public virtual required InvoiceHeader InvoiceHeader { get; set; }
    }
}
