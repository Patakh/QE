using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник услуг терминала офиса
/// </summary>
public partial class SOfficeTerminalService
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
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Папка
    /// </summary>
    public long? SOfficeTerminalFolderId { get; set; }

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

    public virtual SOfficeTerminal SOfficeTerminal { get; set; } = null!;

    public virtual SOfficeTerminalButton? SOfficeTerminalFolder { get; set; }

    public virtual SService SService { get; set; } = null!;
}
