using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Статусы талонов в архиве
/// </summary>
public partial class DTicketStatusArchve
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Талон
    /// </summary>
    public long DTicketArchiveId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public long SStatusId { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public long? SEmployeeId { get; set; }

    /// <summary>
    /// Окно
    /// </summary>
    public long? SOfficeWindowId { get; set; }

    /// <summary>
    /// Окно куда перенаправили
    /// </summary>
    public long? SOfficeWindowIdRedirect { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly? DateAdd { get; set; }

    /// <summary>
    ///  Время
    /// </summary>
    public TimeOnly? TimeAdd { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string? EmployeeNameAdd { get; set; }

    public virtual DTicketArchve DTicketArchive { get; set; } = null!;

    public virtual SEmployee? SEmployee { get; set; }

    public virtual SOfficeWindow? SOfficeWindow { get; set; }

    public virtual SOfficeWindow? SOfficeWindowIdRedirectNavigation { get; set; }

    public virtual SStatus SStatus { get; set; } = null!;
}
