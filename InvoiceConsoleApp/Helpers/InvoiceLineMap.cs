using CsvHelper.Configuration;
using InviceConsoleApp.Service.ViewModels;

public class InvoiceLineMap : ClassMap<InvoiceLineViewModel>
{
    public InvoiceLineMap()
    {
        Map(m => m.InvoiceNumber).Name("Invoice Number");
        Map(m => m.Description).Name("Line description");
        Map(m => m.Quantity).Name("Invoice Quantity");
        Map(m => m.UnitSellingPriceExVAT).Name("Unit selling price ex VAT");
    }
}
