using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bookshelf.Dtos
{
    public partial class BookDto : ObservableObject
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string FileUrl { get; set; }
        public int? ReadingTime { get; set; }
        public int TotalCopies { get; set; }
        public int? PageCount { get; set; }
        public string AuthorNames => Authors != null
            ? string.Join(", ", Authors.Select(a => $"{a.FirstName} {a.LastName}"))
                : "";

        public List<AuthorDto> Authors { get; set; }
        public List<SectionDto> Sections { get; set; }
        public List<GenreDto> Genres { get; set; }

        [ObservableProperty]
        public Bitmap? _image = null;
    }
}
