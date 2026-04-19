using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Logins
{
    public int? Ip { get; set; }

    public string? Usuarios { get; set; }

    public DateOnly? fecha_logins { get; set; }

    public virtual USUARIOS? UsuariosNavigation { get; set; }
}
