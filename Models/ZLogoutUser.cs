using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ZLogoutUser
{
    public int Id { get; set; }

    public bool? LogoutAllUsers { get; set; }
}
