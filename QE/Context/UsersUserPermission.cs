using System;
using System.Collections.Generic;

namespace QE.Context;

public partial class UsersUserPermission
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int PermissionId { get; set; }

    public virtual AuthPermission Permission { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
