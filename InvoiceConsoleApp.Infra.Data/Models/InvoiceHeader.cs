using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceConsoleApp.Infra.Data.Models
{
    public class InvoiceHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(50)]
        public required string InvoiceNumber { get; set; }

        [Required]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(50)]
        public string? Address { get; set; }

        public float? InvoiceTotal { get; set; }

        public virtual ICollection<InvoiceLine>? InvoiceLines { get; set; }
    }
}
