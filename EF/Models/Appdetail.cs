using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Appdetail
{
    public int AppdetailId { get; set; }

    public int AppId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int PpmpId { get; set; }

    public virtual App App { get; set; } = null!;
}
