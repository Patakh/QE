using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник услуг табло офиса
/// </summary>
public partial class SOfficeScoreboardService
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Табло
    /// </summary>
    public long SOfficeScoreboardId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual SOfficeScoreboard SOfficeScoreboard { get; set; } = null!;

    public virtual SService SService { get; set; } = null!;
}
