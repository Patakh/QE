using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник офисов
/// </summary>
public partial class SOffice
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string OfficeName { get; set; } = null!;

    /// <summary>
    /// Адрес
    /// </summary>
    public string OfficeAddress { get; set; } = null!;

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Количество дней для перезаписи
    /// </summary>
    public long? CountDayPrerecord { get; set; }

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicketPrerecord> DTicketPrerecords { get; set; } = new List<DTicketPrerecord>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual ICollection<SEmployee> SEmployees { get; set; } = new List<SEmployee>();

    public virtual ICollection<SOfficePrerecord> SOfficePrerecords { get; set; } = new List<SOfficePrerecord>();

    public virtual ICollection<SOfficeSchedule> SOfficeSchedules { get; set; } = new List<SOfficeSchedule>();

    public virtual ICollection<SOfficeScoreboard> SOfficeScoreboards { get; set; } = new List<SOfficeScoreboard>();

    public virtual ICollection<SOfficeTerminal> SOfficeTerminals { get; set; } = new List<SOfficeTerminal>();

    public virtual ICollection<SOfficeWindow> SOfficeWindows { get; set; } = new List<SOfficeWindow>();
}
