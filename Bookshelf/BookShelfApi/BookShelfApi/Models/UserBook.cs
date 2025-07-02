using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class UserBook
{
    public int UserId { get; set; }

    public int BookId { get; set; }

    public int CurrentPage { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual User User { get; set; } = null!;
}
