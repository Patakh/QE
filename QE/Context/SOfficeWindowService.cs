using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник услуг окна офиса
/// </summary>
public partial class SOfficeWindowService
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Окно
    /// </summary>
    public long SOfficeWindowId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Активность
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual SOfficeWindow SOfficeWindow { get; set; } = null!;

    public virtual SService SService { get; set; } = null!;
}
