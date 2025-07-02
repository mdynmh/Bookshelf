﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShelfApi.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
