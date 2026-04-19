using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class OrderItems
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public virtual Products Product { get; set; } = null!;
}
