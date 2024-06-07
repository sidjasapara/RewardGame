using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiddharthJasapara_550.Model.Models
{
    public class PaginationModel
    {
        public int currentIndex { get; set; }

        public List<TransactionModel> transactionsModel { get; set; }

        public int total { get; set; }

    }
}
