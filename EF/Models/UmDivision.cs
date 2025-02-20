using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmDivision
{
    public int DivisionId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<UmBureau> UmBureaus { get; set; } = new List<UmBureau>();
}
