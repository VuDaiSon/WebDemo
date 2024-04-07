using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public int TotalValue { get; set; }

    public string Status { get; set; } = null!;

    public int CartId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Receiver { get; set; } = null!;

    public int? ShippingFee { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual Cart Cart { get; set; } = null!;

    public virtual User? User { get; set; }
}
