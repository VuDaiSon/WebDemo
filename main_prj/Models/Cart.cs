using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
