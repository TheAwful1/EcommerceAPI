using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Payments
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public virtual Orders Order { get; set; } = null!;
}
