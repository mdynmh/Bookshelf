using System;
using System.Collections.Generic;

namespace BookShelfApi.Models;

public partial class BookFullInfo
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Authors { get; set; }

    public string? Genres { get; set; }
}
