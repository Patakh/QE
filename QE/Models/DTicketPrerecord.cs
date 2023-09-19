using System;
using System.Collections.Generic;

namespace QE.Models;

/// <summary>
/// Талоны предзаписи
/// </summary>
public partial class DTicketPrerecord
{
    /// <summary>
    ///  Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Офис
    /// </summary>
    public long SOfficeId { get; set; }

    /// <summary>
    /// Источник
    /// </summary>
    public long SSourсePrerecordId { get; set; }

    /// <summary>
    /// Префикс
    /// </summary>
    public string ServicePrefix { get; set; } = null!;

    /// <summary>
    /// Приоритет услуги
    /// </summary>
    public long ServicePriority { get; set; }

    /// <summary>
    /// Код предзаписи
    /// </summary>
    public long CodePrerecord { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public long? SEmployeeId { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string? CustomerFullName { get; set; }

    /// <summary>
    /// Номер телефона заявителя
    /// </summary>
    public string? CustomerPhoneNumber { get; set; }

    /// <summary>
    /// Почта заявителя
    /// </summary>
    public string? CustomerEMail { get; set; }

    /// <summary>
    /// СНИЛС Заявителя
    /// </summary>
    public string? CustomerSnils { get; set; }

    /// <summary>
    /// Дата предзаписи
    /// </summary>
    public DateOnly DatePrerecord { get; set; }

    /// <summary>
    /// Начала время предзаписи
    /// </summary>
    public TimeOnly StartTimePrerecord { get; set; }

    /// <summary>
    /// Окончание времени предзаписи
    /// </summary>
    public TimeOnly StopTimePrerecord { get; set; }

    /// <summary>
    /// Подтвреждение того что заявитель пришел и встал в очередь
    /// </summary>
    public bool IsConfirmation { get; set; }

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual SEmployee? SEmployee { get; set; }

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual SService SService { get; set; } = null!;

    public virtual SSourcePrerecord SSourсePrerecord { get; set; } = null!;
}
