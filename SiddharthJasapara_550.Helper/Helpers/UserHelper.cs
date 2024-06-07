using SiddharthJasapara_550.Model.DBContext;
using SiddharthJasapara_550.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiddharthJasapara_550.Helper.Helpers
{
    public class UserHelper
    {
        public static Users CustomToDB(UsersModel userModel)
        {
            Users user = new Users();
            user.id = userModel.id;
            user.username = userModel.username;
            user.email = userModel.email;
            user.password = userModel.password;
            return user;
        }

        public static UsersModel DBToCustom(Users user)
        {
            UsersModel userModel = new UsersModel();
            userModel.id = user.id;
            userModel.username = user.username;
            userModel.email = user.email;
            userModel.password = user.password;

            return userModel;
        }

        public static TransactionModel DBToCustom(Transactions transaction)
        {
            TransactionModel transactionModel = new TransactionModel();
            transactionModel.id = transaction.id;
            transactionModel.user_id = (int)transaction.user_id;
            if (transaction.debited != null)
            {
                transactionModel.debited = (int)transaction.debited;
            }
            else
            {
                transactionModel.debited = 0;
            }
            if (transaction.credited != null)
            {
                transactionModel.credited = (int)transaction.credited;
            }
            else
            {
                transactionModel.credited = 0;
            }
            transactionModel.date = transaction.date;
            transactionModel.balance = transaction.balance;

            return transactionModel;
        }
    }
}
