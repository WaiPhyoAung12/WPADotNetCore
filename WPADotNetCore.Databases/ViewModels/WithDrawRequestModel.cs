using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class WithDrawRequestModel
    {
        public string FullName { get; set; }

        public string MobileNo { get; set; }
        public string Pin { get; set; }
        public decimal  WithDrawAmount { get; set; }
    }
}
