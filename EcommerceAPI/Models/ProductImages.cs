using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class ProductImages
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsMain { get; set; }

    public virtual Products Product { get; set; } = null!;
}
