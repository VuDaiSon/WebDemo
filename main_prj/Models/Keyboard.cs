using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Keyboard
{
    public int? ProductId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string ConnectionType { get; set; } = null!;

    public string? Battery { get; set; }

    public string? Weight { get; set; }

    public string? KeyboardType { get; set; }

    public string? Switch { get; set; }

    public string? Led { get; set; }

    public int KeyboardId { get; set; }

    public virtual Product? Product { get; set; }
}
