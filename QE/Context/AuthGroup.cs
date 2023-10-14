using System;
using System.Collections.Generic;

namespace QE.Context;

public partial class AuthGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AuthGroupPermission> AuthGroupPermissions { get; set; } = new List<AuthGroupPermission>();

    public virtual ICollection<UsersGroup> UsersGroups { get; set; } = new List<UsersGroup>();
}
