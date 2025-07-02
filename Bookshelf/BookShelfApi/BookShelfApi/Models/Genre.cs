using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
