using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class TransactionStatus
{
    public int TransactionStatusId { get; set; }

    public int PageId { get; set; }

    public int ProcessByUserId { get; set; }

    public bool IsCurrent { get; set; }

    public int Count { get; set; }

    public string? Status { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedDate { get; set; }

    public int TransactionId { get; set; }

    public bool IsDone { get; set; }

    public int? WorkstepId { get; set; }

    /// <summary>
    /// Will be disabled(0) if transaction is disapproved. Default value is true(1)
    /// </summary>
    public bool IsActive { get; set; }

    public string? Action { get; set; }

    public virtual UmWorkStep? Workstep { get; set; }
}
