using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Databases.ViewModels
{
    public class TransferRequestModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public decimal TransferBalance { get; set; }
        public string FromMobileNo { get; set; }
        public string ToMobileNo { get; set; }
        public string Pin { get; set; }
        public string? Notes { get; set; }
    }
}
