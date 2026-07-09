using AutoMapper;
using Warehouse.Domain.Entities;
using Warehouse.Application.ViewModels;


namespace Warehouse.Application.Mapping;


public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Supplier, SupplierViewModel>();


        CreateMap<Product, ProductViewModel>()
            .ForMember(
                destination => destination.SupplierName,
                option => option.MapFrom(
                    source => source.Supplier.Name
                )
            );

    }
}