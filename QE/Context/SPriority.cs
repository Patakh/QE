using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник приоритетов
/// </summary>
public partial class SPriority
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string PriorityName { get; set; } = null!;

    /// <summary>
    /// Пазиция приоритета
    /// </summary>
    public long PriorityPosition { get; set; }

    /// <summary>
    /// Активность
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Commentt { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual ICollection<DTicketArchve> DTicketArchves { get; set; } = new List<DTicketArchve>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();
}
