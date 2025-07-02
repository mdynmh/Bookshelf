namespace BookShelfApi.Dtos
{
    public class UserBookDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CurrentPage { get; set; }

        public BookDto Book { get; set; } = null!;

        public List<BookmarkDto> Bookmarks { get; set; } = new();
    }
}
