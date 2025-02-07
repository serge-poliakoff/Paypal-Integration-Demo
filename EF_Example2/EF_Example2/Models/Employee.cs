using System;
using System.Collections.Generic;

namespace EF_Example2.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int EmploiId { get; set; }

    public int WorkPointId { get; set; }

    public DateOnly HireDate { get; set; }

    public virtual Post Emploi { get; set; } = null!;

    public virtual Shop WorkPoint { get; set; } = null!;
}
