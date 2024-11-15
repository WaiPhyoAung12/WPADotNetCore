using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class PinChangeRequestModel
    {
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}
