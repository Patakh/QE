using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник статусов талонов
/// </summary>
public partial class SStatus
{
    /// <summary>
    ///  Наименование
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string StatusName { get; set; } = null!;

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicketStatusArchve> DTicketStatusArchves { get; set; } = new List<DTicketStatusArchve>();

    public virtual ICollection<DTicketStatus> DTicketStatuses { get; set; } = new List<DTicketStatus>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();
}
