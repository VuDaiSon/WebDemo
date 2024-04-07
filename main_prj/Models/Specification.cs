using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Specification
{
    public int SpecId { get; set; }

    public string SpecName { get; set; } = null!;

    public string SpecContent { get; set; } = null!;

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
