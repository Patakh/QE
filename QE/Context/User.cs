using System;
using System.Collections.Generic;

namespace QE.Context;

public partial class User
{
    public long Id { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public bool IsSuperuser { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsStaff { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateJoined { get; set; }

    public virtual ICollection<DjangoAdminLog> DjangoAdminLogs { get; set; } = new List<DjangoAdminLog>();

    public virtual ICollection<UsersGroup> UsersGroups { get; set; } = new List<UsersGroup>();

    public virtual ICollection<UsersUserPermission> UsersUserPermissions { get; set; } = new List<UsersUserPermission>();
}
