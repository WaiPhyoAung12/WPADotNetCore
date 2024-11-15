using System;
using System.Collections.Generic;

namespace WPADotNetCore.Databases.Models;

public partial class TblTransaction
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }
}
