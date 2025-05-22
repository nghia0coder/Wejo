namespace Wejo.Common.SeedWork.Responses;

using SeedWork.Dtos;

public class PagedMessagesResponse<T> where T : MessageDto
{
    public List<T> ReadMessages { get; set; } = new List<T>();
    public List<T> UnreadMessages { get; set; } = new List<T>();
    public PageInfo PageInfo { get; set; } = new PageInfo();
}

public class PageInfo
{
    public bool HasNextPage { get; set; }
    public string? StartCursor { get; set; }
    public string? EndCursor { get; set; }
}
