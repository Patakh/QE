using System;
using System.Collections.Generic;

namespace QE.Context;

public partial class UsersGroup
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int GroupId { get; set; }

    public virtual AuthGroup Group { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
