using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Orders
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();

    public virtual Users User { get; set; } = null!;
}
