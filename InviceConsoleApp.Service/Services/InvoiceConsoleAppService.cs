using AutoMapper;
using InviceConsoleApp.Service.Interfaces;
using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using InvoiceConsoleApp.Infra.Data.Models;
using Microsoft.Extensions.Configuration;

namespace InviceConsoleApp.Service.Services
{
    public class InvoiceConsoleAppService : IInvoiceConsoleAppService
    {
        private readonly IMapper mapper;
        private readonly IInvoiceHeaderRepository invoiceHeaderRepository;
        private readonly IInvoiceLineRepository invoiceLineRepository;
        private readonly IConfiguration configuration;

        public InvoiceConsoleAppService(IMapper _mapper,
            IInvoiceHeaderRepository invoiceHeaderRepository,
            IInvoiceLineRepository invoiceLineRepository,
            IConfiguration configuration)
        {
            mapper = _mapper;
            this.invoiceHeaderRepository = invoiceHeaderRepository;
            this.invoiceLineRepository = invoiceLineRepository;
            this.configuration = configuration;
        }

        public void Create(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders)
        {
            invoiceHeaderRepository.AddRange(mapper.Map<List<InvoiceHeader>>(invoiceHeaders.ToList()));
            invoiceHeaderRepository.SaveChanges();
        }

        public BalanceCheckViewModel GetProductOfQuantityAndUnitSellingPriceExVAT(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders)
        {
            var invoiceLines = invoiceHeaders.Select(x => x?.InvoiceLines).ToList();

            var balanceCheckViewModel = new BalanceCheckViewModel()
            {
                HeaderInvoiceTotal = invoiceHeaders.Sum(x => x.InvoiceTotal),
                ProductOfQuantityAndUnitPrice = invoiceLines.Sum(x => x.Sum(y => y.Quantity * y.UnitSellingPriceExVAT))
            };

            return balanceCheckViewModel;
        }

        public IEnumerable<InvoiceNumberAndQuatityViewModel> GetInvoiceNumberAndSumOfAssociatedLines(IEnumerable<InvoiceHeaderViewModel> invoiceHeaders)
        {
            var invoiceLines = new List<InvoiceNumberAndQuatityViewModel>();

            invoiceHeaders.ToList().ForEach(x =>
            {
                var totalQuantity = x.InvoiceLines.Sum(y => y.Quantity);
                invoiceLines.Add(new InvoiceNumberAndQuatityViewModel
                {
                    InvoiceNumber = x.InvoiceNumber,
                    TotalQuantity = totalQuantity
                });
            });

            return invoiceLines;
        }

        public IEnumerable<InvoiceHeaderViewModel> GetListOfInvoiceNumbers()
        {
            return mapper.Map<List<InvoiceHeaderViewModel>>(invoiceHeaderRepository.GetAll());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}