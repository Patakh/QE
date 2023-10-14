using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник терминалов офисов
/// </summary>
public partial class SOfficeTerminal
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
    ///  Наименование
    /// </summary>
    public string TerminalName { get; set; } = null!;

    /// <summary>
    ///  Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления 
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Количество строк
    /// </summary>
    public long? NumberLines { get; set; }

    /// <summary>
    /// Количество столбцов
    /// </summary>
    public long? NumberColumns { get; set; }

    /// <summary>
    /// IP Адрес
    /// </summary>
    public string? IpAddress { get; set; }

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual ICollection<SOfficeTerminalButton> SOfficeTerminalButtons { get; set; } = new List<SOfficeTerminalButton>();

    public virtual ICollection<SOfficeTerminalService> SOfficeTerminalServices { get; set; } = new List<SOfficeTerminalService>();
}
