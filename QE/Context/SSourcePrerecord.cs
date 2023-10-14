using System;
using System.Collections.Generic;

namespace QE.Context;

/// <summary>
/// Справочник источников пред записи
/// </summary>
public partial class SSourcePrerecord
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string PrerecordName { get; set; } = null!;

    public virtual ICollection<DTicketPrerecord> DTicketPrerecords { get; set; } = new List<DTicketPrerecord>();
}
