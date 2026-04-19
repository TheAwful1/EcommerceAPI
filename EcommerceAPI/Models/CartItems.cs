using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class CartItems
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Carts Cart { get; set; } = null!;

    public virtual Products Product { get; set; } = null!;
}
