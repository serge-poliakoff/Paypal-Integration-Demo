using System;
using System.Collections.Generic;

namespace EF_Example2.Models;

public partial class Post
{
    public int Id { get; set; }

    public int Salary { get; set; }

    public string Schedule { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
