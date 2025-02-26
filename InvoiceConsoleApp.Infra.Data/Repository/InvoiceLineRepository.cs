using InvoiceConsoleApp.Infra.Data.Context;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using InvoiceConsoleApp.Infra.Data.Models;

namespace InvoiceConsoleApp.Infra.Data.Repository
{
    public class InvoiceLineRepository(InvoiceConsoleAppDbContext context) : 
        InvoiceConsoleAppRepository<InvoiceLine>(context), IInvoiceLineRepository
    {
    }
}
