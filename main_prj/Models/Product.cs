using System;
using System.Collections.Generic;

namespace main_prj.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ProductDescription { get; set; }

    public int? Warranty { get; set; }

    public int CategoryId { get; set; } 

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Headphone> Headphones { get; set; } = new List<Headphone>();

    public virtual ICollection<Keyboard> Keyboards { get; set; } = new List<Keyboard>();

    public virtual ICollection<Mouse> Mice { get; set; } = new List<Mouse>();

    public virtual ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();

    public virtual ICollection<Specification> Specifications { get; set; } = new List<Specification>();
}
