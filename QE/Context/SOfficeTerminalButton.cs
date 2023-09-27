using System;
using System.Collections.Generic;

namespace QE.Models;

/// <summary>
/// Справочник кнопок терминала офиса
/// </summary>
public partial class SOfficeTerminalButton
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Терминал
    /// </summary>
    public long SOfficeTerminalId { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string ButtonName { get; set; } = null!;

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Тип. 1 - Меню 2 - Услуга
    /// </summary>
    public long? ButtonType { get; set; }

    /// <summary>
    /// Родительская запись
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long? SServiceId { get; set; }

    public virtual SOfficeTerminal SOfficeTerminal { get; set; } = null!;

    public virtual ICollection<SOfficeTerminalService> SOfficeTerminalServices { get; set; } = new List<SOfficeTerminalService>();
}
