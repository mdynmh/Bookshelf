namespace BookShelfApi.Dtos
{
    public class BookmarkDto
    {
        public int BookmarkId { get; set; }
        public int PageNumber { get; set; }
        public string Note { get; set; } = null!;
    }
}
