using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDriverSignup
{
    public int Id { get; set; }

    public DateTime EnquiryDate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string City { get; set; } = null!;

    public string Suburb { get; set; } = null!;

    public string PostCode { get; set; } = null!;

    public string? Experience { get; set; }
}
