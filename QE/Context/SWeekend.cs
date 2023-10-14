using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Календарь выходных дней
/// </summary>
public partial class SWeekend
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly DateWeekend { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }
}
