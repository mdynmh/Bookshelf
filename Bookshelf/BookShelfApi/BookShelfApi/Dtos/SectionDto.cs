namespace BookShelfApi.Dtos
{
    public class SectionDto
    {
        public int SectionId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int? StartPage { get; set; }
    }
}
