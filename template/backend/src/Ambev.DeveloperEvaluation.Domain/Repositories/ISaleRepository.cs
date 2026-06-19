using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier, including its items
    /// </summary>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its sale number, including its items
    /// </summary>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a page of sales (with items included), optionally filtered and ordered.
    /// </summary>
    /// <param name="page">1-based page number</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Comma-separated list of "field [asc|desc]", e.g. "saleDate desc, saleNumber"</param>
    /// <param name="customerId">Optional filter by customer</param>
    /// <param name="branchId">Optional filter by branch</param>
    /// <param name="status">Optional filter by sale status</param>
    Task<(IReadOnlyList<Sale> Items, int TotalCount)> GetPagedAsync(
        int page,
        int size,
        string? order,
        Guid? customerId,
        Guid? branchId,
        SaleStatus? status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes made to a previously retrieved sale
    /// </summary>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the repository
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
