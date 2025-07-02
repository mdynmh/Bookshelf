using System;
using System.Collections.Generic;

namespace BookShelfApi.Models;

public partial class UserCurrentBook
{
    public int IssueId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public DateTime DueDate { get; set; }
}
