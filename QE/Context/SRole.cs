using System;
using System.Collections.Generic;

namespace QE.Models;

/// <summary>
/// Справочник ролей
/// </summary>
public partial class SRole
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string RoleName { get; set; } = null!;

    public virtual ICollection<SEmployee> SEmployees { get; set; } = new List<SEmployee>();
}
