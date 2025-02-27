using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Context;

namespace InviceConsoleApp.Service.Interfaces
{
    public interface IInvoiceConsoleAppService : IDisposable
    {
        IEnumerable<InvoiceHeaderViewModel> GetListOfInvoiceNumbers();
        void Create(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders);

        IEnumerable<InvoiceNumberAndQuatityViewModel> GetInvoiceNumberAndSumOfAssociatedLines(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders);

        BalanceCheckViewModel GetProductOfQuantityAndUnitSellingPriceExVAT(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders);
    }
}
