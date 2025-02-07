using System;
using System.Collections.Generic;

namespace EF_TestApp1_aka_GangsterManager.Models;

public partial class Gangster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GangId { get; set; }

    public double? WeedSmoken { get; set; }

    public int? GunsInDaPants { get; set; }

    public string Status { get; set; } = null!;

    public virtual Gang Gang { get; set; } = null!;
}
