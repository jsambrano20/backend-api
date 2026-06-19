namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implements the quantity-based discount business rules for sale items:
/// - Below 4 items: no discount allowed.
/// - 4 to 9 items: 10% discount.
/// - 10 to 20 items: 20% discount.
/// - Above 20 items: selling is not allowed.
/// </summary>
public static class SaleDiscountCalculator
{
    public const int MaxQuantityPerProduct = 20;
    private const int MinQuantityForDiscount = 4;
    private const int MinQuantityForExtraDiscount = 10;
    private const decimal StandardDiscount = 0.10m;
    private const decimal ExtraDiscount = 0.20m;

    /// <summary>
    /// Calculates the discount percentage (0 to 1) applicable for the given quantity.
    /// </summary>
    /// <exception cref="DomainException">Thrown when quantity exceeds the maximum allowed per product.</exception>
    public static decimal CalculateDiscount(int quantity)
    {
        if (quantity > MaxQuantityPerProduct)
            throw new DomainException($"It's not possible to sell above {MaxQuantityPerProduct} identical items");

        if (quantity >= MinQuantityForExtraDiscount)
            return ExtraDiscount;

        if (quantity >= MinQuantityForDiscount)
            return StandardDiscount;

        return 0m;
    }
}
