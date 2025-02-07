using System;
using System.Collections.Generic;

namespace EF_Example2.Models;

public partial class Shop
{
    public int Id { get; set; }

    public string Adress { get; set; } = null!;

    public int? LastRevenue { get; set; }

    public int Materials { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
