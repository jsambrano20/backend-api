using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single product line within a sale.
/// Uses the External Identities pattern: the product is referenced by <see cref="ProductId"/>
/// while <see cref="ProductName"/> is a denormalized copy of the product description at the time of sale.
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }

    /// <summary>
    /// External identity of the product in the Products domain.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Denormalized product description, captured at the time of sale.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount percentage applied to this item, expressed as a fraction (e.g. 0.10 = 10%).
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item after discount: Quantity * UnitPrice * (1 - Discount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    public bool IsCancelled { get; set; }

    /// <summary>
    /// Recalculates <see cref="Discount"/> and <see cref="TotalAmount"/> based on the current quantity and unit price.
    /// Enforces the quantity-based discount business rules.
    /// </summary>
    public void ApplyQuantity(int quantity, decimal unitPrice)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = SaleDiscountCalculator.CalculateDiscount(quantity);
        RecalculateTotal();
    }

    public void RecalculateTotal()
    {
        TotalAmount = Quantity * UnitPrice * (1 - Discount);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}
