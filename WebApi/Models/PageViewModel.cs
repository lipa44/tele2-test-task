namespace WebApi.Models;

public record PageViewModel
{
    public int PageNumber { get; }
    public int TotalPages { get; }

    public PageViewModel(int count, int pageNumber, int takeAmount)
    {
        PageNumber = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double) takeAmount);
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}