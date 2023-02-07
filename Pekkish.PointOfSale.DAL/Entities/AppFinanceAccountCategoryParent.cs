using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppFinanceAccountCategoryParent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AppFinanceAccountCategory> AppFinanceAccountCategories { get; } = new List<AppFinanceAccountCategory>();
}
