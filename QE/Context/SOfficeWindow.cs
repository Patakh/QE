using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник окон офисов
/// </summary>
public partial class SOfficeWindow
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
    public string WindowName { get; set; } = null!;

    /// <summary>
    /// IP Адрес
    /// </summary>
    public string? WindowIp { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicketStatusArchve> DTicketStatusArchveSOfficeWindowIdRedirectNavigations { get; set; } = new List<DTicketStatusArchve>();

    public virtual ICollection<DTicketStatusArchve> DTicketStatusArchveSOfficeWindows { get; set; } = new List<DTicketStatusArchve>();

    public virtual ICollection<DTicketStatus> DTicketStatusSOfficeWindowIdTransferredNavigations { get; set; } = new List<DTicketStatus>();

    public virtual ICollection<DTicketStatus> DTicketStatusSOfficeWindows { get; set; } = new List<DTicketStatus>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual ICollection<SOfficeWindowService> SOfficeWindowServices { get; set; } = new List<SOfficeWindowService>();
}
