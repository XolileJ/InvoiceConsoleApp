using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Context;

namespace InviceConsoleApp.Service.Interfaces
{
    public interface IInvoiceConsoleAppService : IDisposable
    {
        void Create(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders);

        float GetProductOfQuantityAndUnitSellingPriceExVAT();
    }
}
