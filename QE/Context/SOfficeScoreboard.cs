using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник информационных табло офисов
/// </summary>
public partial class SOfficeScoreboard
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
    /// Наименование
    /// </summary>
    public string ScoreboardName { get; set; } = null!;

    /// <summary>
    /// Активность
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Дата и время добавления 
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Ключ
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Ip адрес 
    /// </summary>
    public string? ScoreboardIp { get; set; }

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual ICollection<SOfficeScoreboardMultimedium> SOfficeScoreboardMultimedia { get; set; } = new List<SOfficeScoreboardMultimedium>();

    public virtual ICollection<SOfficeScoreboardService> SOfficeScoreboardServices { get; set; } = new List<SOfficeScoreboardService>();
}
