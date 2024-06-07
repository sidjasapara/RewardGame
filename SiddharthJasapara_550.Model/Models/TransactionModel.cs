using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiddharthJasapara_550.Model.Models
{
    public class TransactionModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int debited { get; set; }
        public int credited { get; set; }
        public DateTime date { get; set; }
        public int balance { get; set; }
    }
}
