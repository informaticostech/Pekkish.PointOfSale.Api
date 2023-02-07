using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class HubspotVendorTerm
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? Description { get; set; }

    public string? Fullfillment { get; set; }

    public string? Dietary { get; set; }

    public string? Cuisine { get; set; }

    public string? Instagram { get; set; }

    public string? ContactName { get; set; }

    public string? ContactSurname { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public string? AddressStreet { get; set; }

    public string? AddressSuburb { get; set; }

    public string? AddressCity { get; set; }

    public string? AddressPostCode { get; set; }
}
