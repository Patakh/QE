using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник папок услуг терминала офиса
/// </summary>
public partial class SOfficeTerminalFolder
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
    public string FolderName { get; set; } = null!;

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual SOfficeTerminal SOfficeTerminal { get; set; } = null!;

    public virtual ICollection<SOfficeTerminalService> SOfficeTerminalServices { get; set; } = new List<SOfficeTerminalService>();
}
