namespace InviceConsoleApp.Service.ViewModels
{
    public class InvoiceNumberAndQuatityViewModel
    {
        public required string InvoiceNumber { get; set; }

        public double TotalQuantity { get; set; }
        public double HeaderInvoiceTotal { get; set; }
        public double ProductOfQuantityAndUnitPrice { get; set; }
    }
}
