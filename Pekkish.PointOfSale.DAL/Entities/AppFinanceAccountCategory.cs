using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppFinanceAccountCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int FinanceAccountCategoryParentId { get; set; }

    public bool? IsVisible { get; set; }

    public virtual ICollection<AppFinanceAccount> AppFinanceAccounts { get; } = new List<AppFinanceAccount>();

    public virtual AppFinanceAccountCategoryParent FinanceAccountCategoryParent { get; set; } = null!;
}
