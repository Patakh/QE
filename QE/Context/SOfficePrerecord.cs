using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник ограничения по количеству талонов офисов
/// </summary>
public partial class SOfficePrerecord
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Количество талонов 
    /// </summary>
    public long PrerecordCount { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Офис
    /// </summary>
    public long SOfficeId { get; set; }

    /// <summary>
    /// День недели
    /// </summary>
    public long SDayWeekId { get; set; }

    /// <summary>
    /// Начала время предзаписи
    /// </summary>
    public TimeOnly StartTimePrerecord { get; set; }

    /// <summary>
    /// Окончание времени предзаписи
    /// </summary>
    public TimeOnly StopTimePrerecord { get; set; }

    public virtual SOffice SOffice { get; set; } = null!;
}
