using InvoiceConsoleApp.Infra.Data.Context;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using InvoiceConsoleApp.Infra.Data.Models;

namespace InvoiceConsoleApp.Infra.Data.Repository
{
    public class InvoiceHeaderRepository(InvoiceConsoleAppDbContext context) :
        InvoiceConsoleAppRepository<InvoiceHeader>(context), IInvoiceHeaderRepository
    {
        public IEnumerable<InvoiceHeader> GetAllInvoices()
        {
            var test = context.InvoiceHeader
                              .Select(x = new { })
                              .ToList();

            return new List<InvoiceHeader>();
        }
    }
}
