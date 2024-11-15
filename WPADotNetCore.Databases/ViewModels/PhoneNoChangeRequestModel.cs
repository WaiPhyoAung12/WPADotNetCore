using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class PhoneNoChangeRequestModel
    {
        public string FullName { get; set; }
        public string Pin { get; set; }
        public string OldPhoneNo { get; set; }
        public string NewPhoneNo { get; set; }
    }
}
