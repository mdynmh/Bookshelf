namespace BookShelfApi.Dtos
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? FileUrl { get; set; }
        public int TotalCopies { get; set; }
        public int? PageCount { get; set; }
        public int? ReadingTime { get; set; }

        public List<AuthorDto> Authors { get; set; } = new();
        public List<GenreDto> Genres { get; set; } = new();
        public List<SectionDto> Sections { get; set; } = new();
    }
}
