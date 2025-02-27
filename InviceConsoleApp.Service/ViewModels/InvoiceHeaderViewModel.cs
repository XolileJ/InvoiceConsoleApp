namespace InviceConsoleApp.Service.ViewModels
{
    public class InvoiceHeaderViewModel
    {
        public int InvoiceId { get; set; }

        public required string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string? Address { get; set; }

        public double InvoiceTotal { get; set; }

        public virtual ICollection<InvoiceLineViewModel>? InvoiceLines { get; set; }
    }
}
