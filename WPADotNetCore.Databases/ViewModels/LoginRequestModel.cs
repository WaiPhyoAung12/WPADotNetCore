using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class LoginRequestModel
    {
        public string FullName { get; set; } = null!;
        public string Pin { get; set; } = null!;
    }
}
