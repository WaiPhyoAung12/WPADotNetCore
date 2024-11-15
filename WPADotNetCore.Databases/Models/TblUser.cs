using System;
using System.Collections.Generic;

namespace WPADotNetCore.Databases.Models;

public partial class TblUser
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? MobileNo { get; set; }

    public decimal Balance { get; set; }

    public string? Pin { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
