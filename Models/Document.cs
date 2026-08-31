using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class Document
{
    public int Id { get; set; }

    public int? PropertyId { get; set; }

    public string? Document1 { get; set; }

    public DateTime? DocumentDate { get; set; }

    public string? Location { get; set; }

    public int? Type { get; set; }

    public DateTime? AddDate { get; set; }

    public string? AddUser { get; set; }

    public DateTime? ModDate { get; set; }

    public string? ModUser { get; set; }

    public virtual ZDocumentType? TypeNavigation { get; set; }
}
