using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Sale"/> entity, covering item discounting,
/// total recalculation and cancellation behavior.
/// </summary>
public class SaleTests
{
    private static Sale CreateSale()
    {
        return new Sale
        {
            SaleNumber = "SALE-0001",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Main Branch"
        };
    }

    [Fact(DisplayName = "Adding an item with quantity 3 should not apply a discount")]
    public void Given_QuantityBelowFour_When_AddingItem_Then_NoDiscountApplied()
    {
        var sale = CreateSale();

        var item = sale.AddItem(Guid.NewGuid(), "Product A", 3, 10m);

        item.Discount.Should().Be(0m);
        item.TotalAmount.Should().Be(30m);
        sale.TotalAmount.Should().Be(30m);
    }

    [Fact(DisplayName = "Adding an item with quantity 5 should apply a 10% discount")]
    public void Given_QuantityOfFive_When_AddingItem_Then_TenPercentDiscountApplied()
    {
        var sale = CreateSale();

        var item = sale.AddItem(Guid.NewGuid(), "Product A", 5, 10m);

        item.Discount.Should().Be(0.10m);
        item.TotalAmount.Should().Be(45m);
    }

    [Fact(DisplayName = "Adding an item with quantity 15 should apply a 20% discount")]
    public void Given_QuantityOfFifteen_When_AddingItem_Then_TwentyPercentDiscountApplied()
    {
        var sale = CreateSale();

        var item = sale.AddItem(Guid.NewGuid(), "Product A", 15, 10m);

        item.Discount.Should().Be(0.20m);
        item.TotalAmount.Should().Be(120m);
    }

    [Fact(DisplayName = "Adding an item with quantity above 20 should throw")]
    public void Given_QuantityAboveTwenty_When_AddingItem_Then_ShouldThrowDomainException()
    {
        var sale = CreateSale();

        var act = () => sale.AddItem(Guid.NewGuid(), "Product A", 21, 10m);

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Cancelling the sale should cancel all items and recalculate total to zero")]
    public void Given_SaleWithItems_When_Cancelled_Then_AllItemsCancelledAndTotalIsZero()
    {
        var sale = CreateSale();
        sale.AddItem(Guid.NewGuid(), "Product A", 5, 10m);
        sale.AddItem(Guid.NewGuid(), "Product B", 2, 20m);

        sale.Cancel();

        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.Items.Should().OnlyContain(i => i.IsCancelled);
        sale.TotalAmount.Should().Be(0m);
    }

    [Fact(DisplayName = "Cancelling an already cancelled sale should throw")]
    public void Given_CancelledSale_When_CancelledAgain_Then_ShouldThrowDomainException()
    {
        var sale = CreateSale();
        sale.AddItem(Guid.NewGuid(), "Product A", 5, 10m);
        sale.Cancel();

        var act = () => sale.Cancel();

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Cancelling a single item should recalculate the sale total excluding it")]
    public void Given_SaleWithMultipleItems_When_CancellingOneItem_Then_TotalExcludesIt()
    {
        var sale = CreateSale();
        var item1 = sale.AddItem(Guid.NewGuid(), "Product A", 5, 10m); // 45
        sale.AddItem(Guid.NewGuid(), "Product B", 2, 20m); // 40

        sale.CancelItem(item1.Id);

        item1.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(40m);
        sale.Status.Should().Be(SaleStatus.NotCancelled);
    }

    [Fact(DisplayName = "Cancelling an unknown item should throw")]
    public void Given_Sale_When_CancellingUnknownItem_Then_ShouldThrowKeyNotFoundException()
    {
        var sale = CreateSale();
        sale.AddItem(Guid.NewGuid(), "Product A", 5, 10m);

        var act = () => sale.CancelItem(Guid.NewGuid());

        act.Should().Throw<KeyNotFoundException>();
    }
}
