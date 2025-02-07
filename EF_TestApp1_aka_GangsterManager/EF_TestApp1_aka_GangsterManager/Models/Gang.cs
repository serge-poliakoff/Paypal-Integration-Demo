using System;
using System.Collections.Generic;

namespace EF_TestApp1_aka_GangsterManager.Models;

public partial class Gang
{
    public int Id { get; set; }

    public string GangName { get; set; } = null!;

    public virtual ICollection<Gangster> Gangsters { get; set; } = new List<Gangster>();
}
