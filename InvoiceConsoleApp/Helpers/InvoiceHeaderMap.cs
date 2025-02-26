using CsvHelper.Configuration;
using InviceConsoleApp.Service.ViewModels;
public class InvoiceHeaderMap : ClassMap<InvoiceHeaderViewModel>
{
    public InvoiceHeaderMap()
    {
        Map(m => m.InvoiceNumber).Name("Invoice Number");
        Map(m => m.InvoiceDate).Name("Invoice Date").TypeConverterOption.Format("dd/MM/yyyy HH:mm");
        Map(m => m.Address).Name("Address");
        Map(m => m.InvoiceTotal).Name("Invoice Total Ex VAT");
    }
}
