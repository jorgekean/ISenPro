using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmUserAccountRole
{
    public int UserAccountId { get; set; }

    public string RoleCode { get; set; } = null!;

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmRole RoleCodeNavigation { get; set; } = null!;

    public virtual UmUserAccount UserAccount { get; set; } = null!;
}
