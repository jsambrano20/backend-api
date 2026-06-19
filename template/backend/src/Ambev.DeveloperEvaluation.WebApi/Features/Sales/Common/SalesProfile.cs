using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

/// <summary>
/// Shared mappings reused by every Sales feature, registered once to avoid
/// AutoMapper duplicate type-map errors across multiple feature profiles.
/// </summary>
public class SalesProfile : Profile
{
    public SalesProfile()
    {
        CreateMap<SaleItemRequest, Application.Sales.Common.SaleItemRequest>();
        CreateMap<SaleItemResult, SaleItemResponse>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<GetSaleResult, GetSaleResponse>();
    }
}
