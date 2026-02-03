namespace WebApi.Common.Core.Search.Models;

public class SearchResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public PaginationSettings PaginationSettings { get; set; }
    public bool IsPaginated => PaginationSettings is not null;
}