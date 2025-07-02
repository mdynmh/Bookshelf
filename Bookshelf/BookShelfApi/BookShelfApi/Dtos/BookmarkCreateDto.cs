namespace BookShelfApi.Dtos
{
    public class BookmarkCreateDto
    {
        public int UserId { get; set; }

        public int BookId { get; set; }
        public int PageNumber { get; set; }

        public string Note { get; set; } = null!;
    }
}
