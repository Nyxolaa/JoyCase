using System;
using System.Collections.Generic;

namespace JoyCase.Data.Data;

public partial class Permission
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
