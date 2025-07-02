using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class IssuedBook
{
    public int IssueId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
