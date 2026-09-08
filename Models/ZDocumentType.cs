using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ZDocumentType
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
