using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Headphone
{
    public int? ProductId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Compatibilities { get; set; } = null!;

    public string ConnectionType { get; set; } = null!;

    public string? Battery { get; set; }

    public string? Accessories { get; set; }

    public string? Microphone { get; set; }

    public string? HeadphoneType { get; set; }

    public string? Color { get; set; }

    public int HeadphoneId { get; set; }

    public virtual Product? Product { get; set; }
}
