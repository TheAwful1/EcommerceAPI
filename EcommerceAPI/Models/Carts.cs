using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Carts
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual Users User { get; set; } = null!;
}
