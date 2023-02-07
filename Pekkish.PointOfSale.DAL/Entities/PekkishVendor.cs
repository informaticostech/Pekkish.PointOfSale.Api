using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishVendor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Header { get; set; }

    public string? Slug { get; set; }

    public string? Timezone { get; set; }

    public int CityId { get; set; }

    public bool IsOpen { get; set; }

    public string? Address { get; set; }

    public string? AddressNotes { get; set; }

    public string? Cellphone { get; set; }

    public string? EmailAddress { get; set; }

    public bool IsHalaal { get; set; }

    public bool IsKeto { get; set; }

    public bool IsVeg { get; set; }

    public bool IsVegan { get; set; }

    public int? OwnerId { get; set; }

    public string? Phone { get; set; }

    public string? PostCode { get; set; }

    public string? About { get; set; }

    public string? Logo { get; set; }

    public int MenuCount { get; set; }
}
