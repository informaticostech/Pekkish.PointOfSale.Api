using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishPekkishUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int LoginType { get; set; }

    public string? SocialId { get; set; }

    public string? PhotoUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CellphoneNumber { get; set; }

    public string? CountryCode { get; set; }

    public int CityId { get; set; }

    public string? Address { get; set; }

    public string? AddressNotes { get; set; }

    public string? PostCode { get; set; }

    public string? EmailAddress { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int? VendorId { get; set; }

    public int? PekkishUserTypeId { get; set; }
}
