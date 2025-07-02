using System;
using System.Collections.Generic;

namespace BookShelfApi.Models;

public partial class Student
{
    public int UserId { get; set; }

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
