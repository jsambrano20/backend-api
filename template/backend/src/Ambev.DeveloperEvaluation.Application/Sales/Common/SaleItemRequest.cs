namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// Represents a sale item as provided when creating or updating a sale.
/// Follows the External Identities pattern: <see cref="ProductId"/> references the Products domain,
/// while <see cref="ProductName"/> is the denormalized product description.
/// </summary>
public class SaleItemRequest
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
