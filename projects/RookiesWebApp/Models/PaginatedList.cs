namespace RookiesWebApp.Models;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public int PageIndex { get; }
    public int TotalPages { get; }

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex < TotalPages - 1;
}

public static class PaginatedListExtensions
{
    public static PaginatedList<T> Create<T>(this ICollection<T> source, int pageIndex, int pageSize) where T : class
    {
        var count = source.Count;
        var items = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}