using AutoMapper;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Application.Products.Queries.GetProductById;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Application.Suppliers.Queries.GetSupplierById;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Supplier, SupplierViewModel>();

        CreateMap<Product, ProductViewModel>()
            .ForMember(
                destination => destination.Quantity,
                option => option.MapFrom(source => source.QuantityInStock)
            )
            .ForMember(
                destination => destination.SupplierName,
                option => option.MapFrom(source => source.Supplier != null
                    ? source.Supplier.Name
                    : source.SupplierName
                )
            );


        CreateMap<CreateProductResponse, ProductViewModel>();

        CreateMap<GetProductByIdResponse, ProductViewModel>();


        CreateMap<CreateSupplierResponse, SupplierViewModel>();

        CreateMap<GetSupplierByIdResponse, SupplierViewModel>();
    }
}