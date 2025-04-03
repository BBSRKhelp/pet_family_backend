namespace PetFamily.Core.Models;

public record PagedList<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public long TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber * PageSize < TotalCount;
} 