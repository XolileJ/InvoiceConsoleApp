using AutoMapper;
using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Models;

namespace InviceConsoleApp.Service.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Map entity framework objects to service models
        /// </summary>
        public DomainToViewModelMappingProfile()
        {
            CreateMap<InvoiceHeader, InvoiceHeaderViewModel>()
                .ReverseMap();
            CreateMap<InvoiceLine, InvoiceLineViewModel>()
                .ReverseMap();
        }
    }
}
