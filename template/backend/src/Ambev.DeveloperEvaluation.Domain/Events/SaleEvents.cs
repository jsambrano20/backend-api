using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Raised when a new sale is created. Not published to any broker; handlers may log it.
/// </summary>
public class SaleCreatedEvent
{
    public Sale Sale { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}

/// <summary>
/// Raised when an existing sale is modified.
/// </summary>
public class SaleModifiedEvent
{
    public Sale Sale { get; }

    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }
}

/// <summary>
/// Raised when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent
{
    public Sale Sale { get; }

    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }
}

/// <summary>
/// Raised when a single item within a sale is cancelled.
/// </summary>
public class ItemCancelledEvent
{
    public Sale Sale { get; }
    public SaleItem Item { get; }

    public ItemCancelledEvent(Sale sale, SaleItem item)
    {
        Sale = sale;
        Item = item;
    }
}
