using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int? StartPage { get; set; }

    public virtual Book Book { get; set; } = null!;
}
