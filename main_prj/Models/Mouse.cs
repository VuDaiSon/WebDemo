using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Mouse
{
    public int? ProductId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public string Resolution { get; set; } = null!;

    public string ConnectionType { get; set; } = null!;

    public string? Battery { get; set; }

    public string? Color { get; set; }

    public int MouseId { get; set; }

    public virtual Product? Product { get; set; }
}
