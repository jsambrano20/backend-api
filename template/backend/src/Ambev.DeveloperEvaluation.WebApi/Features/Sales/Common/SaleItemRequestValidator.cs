using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public class SaleItemRequestValidator : AbstractValidator<SaleItemRequest>
{
    public SaleItemRequestValidator()
    {
        RuleFor(i => i.ProductId).NotEmpty();
        RuleFor(i => i.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(i => i.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(SaleDiscountCalculator.MaxQuantityPerProduct)
            .WithMessage($"It's not possible to sell above {SaleDiscountCalculator.MaxQuantityPerProduct} identical items.");
        RuleFor(i => i.UnitPrice).GreaterThan(0);
    }
}
