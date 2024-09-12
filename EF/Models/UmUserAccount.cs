using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class UmUserAccount
{
    public int UserAccountId { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? ExpireDate { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int PersonId { get; set; }

    public virtual ICollection<UmUserAccountRole> UmUserAccountRoles { get; set; } = new List<UmUserAccountRole>();
}
