using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class UspsAddress
{
    public int Id { get; set; }

    public string AddressId { get; set; } = null!;

    public string? Addressee { get; set; }

    public bool? UsableMatch { get; set; }

    public string? Street { get; set; }

    public string? Street2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public string? Zip4 { get; set; }

    public string? Longitude { get; set; }

    public string? Latitude { get; set; }

    public string AddressInfoXml { get; set; } = null!;

    public DateTime ModDateTime { get; set; }

    public string AddressSource { get; set; } = null!;

    public int? NumMatches { get; set; }

    public bool? ExcludeFromAddressLookup { get; set; }
}
