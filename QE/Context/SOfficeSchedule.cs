using System;
using System.Collections.Generic;

namespace QE.Models;

/// <summary>
/// Справочник графиков работ офисов
/// </summary>
public partial class SOfficeSchedule
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Офис
    /// </summary>
    public long SOfficeId { get; set; }

    /// <summary>
    /// День недели
    /// </summary>
    public long SDayWeekId { get; set; }

    /// <summary>
    /// Время начала
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    ///  Время окончание
    /// </summary>
    public TimeOnly StopTime { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual SDayWeek SDayWeek { get; set; } = null!;

    public virtual SOffice SOffice { get; set; } = null!;
}
