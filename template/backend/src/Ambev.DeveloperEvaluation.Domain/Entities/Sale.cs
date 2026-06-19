using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale record.
/// Uses the External Identities pattern: Customer and Branch are referenced by id while their
/// descriptions are denormalized (<see cref="CustomerName"/>, <see cref="BranchName"/>) at the time of sale.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Human-readable, unique sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; }

    /// <summary>
    /// External identity of the customer in the Customers domain.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Denormalized customer description, captured at the time of sale.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// External identity of the branch in the Branches domain.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Denormalized branch description, captured at the time of sale.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    public SaleStatus Status { get; set; }

    /// <summary>
    /// Sum of the total amount of every non-cancelled item.
    /// </summary>
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<SaleItem> Items { get; set; } = [];

    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleStatus.NotCancelled;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Adds a new item to the sale, applying the quantity-based discount business rules.
    /// </summary>
    public SaleItem AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        var item = new SaleItem
        {
            SaleId = Id,
            ProductId = productId,
            ProductName = productName
        };
        item.ApplyQuantity(quantity, unitPrice);

        Items.Add(item);
        RecalculateTotal();
        return item;
    }

    public void RecalculateTotal()
    {
        TotalAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
    }

    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is already cancelled");

        Status = SaleStatus.Cancelled;
        foreach (var item in Items.Where(i => !i.IsCancelled))
            item.Cancel();

        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
    }

    public void CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new KeyNotFoundException($"Item with ID {itemId} not found in this sale");

        if (item.IsCancelled)
            throw new DomainException("Item is already cancelled");

        item.Cancel();
        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
    }
}
