using AutoMapper;
using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.Data.Models;

namespace InviceConsoleApp.Service.AutoMapper
{
    internal class ReverseMapDomainAndViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Map entity framework objects to service models
        /// </summary>
        public ReverseMapDomainAndViewModelMappingProfile()
        {
            CreateMap<InvoiceHeader, InvoiceHeaderViewModel>()
                .ReverseMap();
            CreateMap<InvoiceLine, InvoiceLineViewModel>()
                .ReverseMap();
        }
    }
}
