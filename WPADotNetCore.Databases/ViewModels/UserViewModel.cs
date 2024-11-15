using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class UserViewModel
    {
        public string FullName { get; set; } = null!;
        public string? MobileNo { get; set; }
        public string? Pin { get; set; }
    }
}
