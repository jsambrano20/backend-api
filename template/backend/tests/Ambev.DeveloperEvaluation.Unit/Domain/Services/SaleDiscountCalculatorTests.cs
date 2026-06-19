using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

/// <summary>
/// Contains unit tests for the <see cref="SaleDiscountCalculator"/> quantity-based discount rules.
/// </summary>
public class SaleDiscountCalculatorTests
{
    [Theory(DisplayName = "Quantities below 4 should not receive a discount")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Given_QuantityBelowFour_When_CalculatingDiscount_Then_ShouldReturnZero(int quantity)
    {
        var discount = SaleDiscountCalculator.CalculateDiscount(quantity);

        discount.Should().Be(0m);
    }

    [Theory(DisplayName = "Quantities between 4 and 9 should receive a 10% discount")]
    [InlineData(4)]
    [InlineData(7)]
    [InlineData(9)]
    public void Given_QuantityBetweenFourAndNine_When_CalculatingDiscount_Then_ShouldReturnTenPercent(int quantity)
    {
        var discount = SaleDiscountCalculator.CalculateDiscount(quantity);

        discount.Should().Be(0.10m);
    }

    [Theory(DisplayName = "Quantities between 10 and 20 should receive a 20% discount")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void Given_QuantityBetweenTenAndTwenty_When_CalculatingDiscount_Then_ShouldReturnTwentyPercent(int quantity)
    {
        var discount = SaleDiscountCalculator.CalculateDiscount(quantity);

        discount.Should().Be(0.20m);
    }

    [Fact(DisplayName = "Quantities above 20 should not be allowed")]
    public void Given_QuantityAboveTwenty_When_CalculatingDiscount_Then_ShouldThrowDomainException()
    {
        var act = () => SaleDiscountCalculator.CalculateDiscount(21);

        act.Should().Throw<DomainException>()
            .WithMessage("It's not possible to sell above 20 identical items");
    }
}
