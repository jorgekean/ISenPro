using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VTransactionHistory
{
    public int TransactionStatusId { get; set; }

    public int TransactionId { get; set; }

    public int PageId { get; set; }

    public string? Description { get; set; }

    public string UserId { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? Remarks { get; set; }

    public string? Action { get; set; }

    public DateTime CreatedDate { get; set; }
}
