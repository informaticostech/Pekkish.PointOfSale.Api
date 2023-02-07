using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class HubspotDriverEnquiry
{
    public int Id { get; set; }

    public DateTime EnquiryDate { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? City { get; set; }

    public string? Suburb { get; set; }

    public string? PostCode { get; set; }

    public string? Experience { get; set; }
}
