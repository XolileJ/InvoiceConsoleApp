using AutoMapper;
using InviceConsoleApp.Service.Interfaces;
using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Context;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using InvoiceConsoleApp.Infra.Data.Models;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.IO;

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

        public float GetProductOfQuantityAndUnitSellingPriceExVAT()
        {
            var test = invoiceHeaderRepository.GetAllInvoices();

            var invoiceHeaders = invoiceHeaderRepository
                                .GetAll()
                                .Select(x => new
                                {
                                    Quantity = x.InvoiceTotal != null ? (float)x.InvoiceTotal : 0,
                                    InvoiceLines = new List<InvoiceLine>()
                                    //InvoiceLines = x.InvoiceLines?.Select(line => new
                                    //{
                                    //    Quantity = line.Quantity != null ? (float)line.Quantity : 0,
                                    //    UnitPrice = line.UnitSellingPriceExVAT != null ? (float)line.UnitSellingPriceExVAT : 0,
                                    //    Total = line.Quantity != null && line.UnitSellingPriceExVAT != null ?
                                    //            (float)(line.Quantity * line.UnitSellingPriceExVAT) : 0
                                    //}).ToList()
                                })
                                .ToList();
            return 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}