using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник фото видео табло офиса
/// </summary>
public partial class SOfficeScoreboardMultimedium
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
    /// Наименование
    /// </summary>
    public string MultimediaName { get; set; } = null!;

    /// <summary>
    /// Путь к фото видео
    /// </summary>
    public string MultimediaPath { get; set; } = null!;

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

    public virtual SOfficeScoreboard SOfficeScoreboard { get; set; } = null!;
}
