using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Users
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Carts? Carts { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    public virtual ICollection<Roles> Role { get; set; } = new List<Roles>();
}
