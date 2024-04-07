using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Monitor
{
    public int? ProductId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public decimal Size { get; set; }

    public string AspectRatio { get; set; } = null!;

    public string Resolution { get; set; } = null!;

    public int RefreshRate { get; set; }

    public int MonitorId { get; set; }

    public virtual Product? Product { get; set; }
}
