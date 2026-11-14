namespace WebApiExampleP42.Models.DTO;

public class PaginatedResult<T> where T : class
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public ICollection<T> Models { get; set; }
}
