using InvoiceConsoleApp.Infra.Data.Models;

namespace InvoiceConsoleApp.Infra.Data.Interfaces
{
    public interface IInvoiceHeaderRepository : IInvoiceConsoleAppRepository<InvoiceHeader>
    {
        IEnumerable<string> GetAllInvoiceNumbers();
    }
}
