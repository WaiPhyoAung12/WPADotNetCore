using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class DepositRequestModel
    {
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public decimal DepositAmount { get; set; }
    }
}
