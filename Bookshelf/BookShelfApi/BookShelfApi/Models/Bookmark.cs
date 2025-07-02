using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class Bookmark
{
    public int BookmarkId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public int PageNumber { get; set; }

    public string Note { get; set; } = null!;

    public virtual UserBook UserBook { get; set; } = null!;
}
