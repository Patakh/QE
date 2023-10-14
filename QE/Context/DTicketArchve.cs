using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Талоны в архиве
/// </summary>
public partial class DTicketArchve
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
    /// Терминал
    /// </summary>
    public long SOfficeTerminalId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Префикс
    /// </summary>
    public string ServicePrefix { get; set; } = null!;

    /// <summary>
    /// Приоритет Заявителя
    /// </summary>
    public long? SPriorityId { get; set; }

    /// <summary>
    /// Номер талона
    /// </summary>
    public string TicketNumber { get; set; } = null!;

    /// <summary>
    /// Полный номер талона
    /// </summary>
    public string TicketNumberFull { get; set; } = null!;

    /// <summary>
    /// Связь с предзаписью
    /// </summary>
    public long? DTicketPrerecordId { get; set; }

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
    /// Дата постановки в очередь
    /// </summary>
    public DateOnly DateRegistration { get; set; }

    /// <summary>
    /// Время постановки в очередь
    /// </summary>
    public TimeOnly TimeRegistration { get; set; }

    /// <summary>
    /// Время окончания обсужевания
    /// </summary>
    public TimeOnly TimeStopService { get; set; }

    /// <summary>
    ///  Время вызова
    /// </summary>
    public TimeOnly TimeCall { get; set; }

    /// <summary>
    /// Время начала обслужевания
    /// </summary>
    public TimeOnly TimeStartService { get; set; }

    /// <summary>
    /// Воемя ожидания
    /// </summary>
    public TimeOnly TimeWaiting { get; set; }

    public virtual DTicketPrerecord? DTicketPrerecord { get; set; }

    public virtual ICollection<DTicketStatusArchve> DTicketStatusArchves { get; set; } = new List<DTicketStatusArchve>();

    public virtual SEmployee? SEmployee { get; set; }

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual SOfficeTerminal SOfficeTerminal { get; set; } = null!;

    public virtual SOfficeWindow? SOfficeWindow { get; set; }

    public virtual SPriority? SPriority { get; set; }

    public virtual SService SService { get; set; } = null!;

    public virtual SStatus SStatus { get; set; } = null!;
}
