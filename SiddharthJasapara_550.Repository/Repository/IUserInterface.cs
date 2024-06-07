using SiddharthJasapara_550.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiddharthJasapara_550.Repository.Repository
{
    public interface IUserInterface
    {
        bool UserAdded(UsersModel loginModel);
        UsersModel ExistingUser(UserLoginModel loginModel);
        List<TransactionModel> GetTransactions(int user_id);
        int GamePlay(int reward, int user_id);
        int GetBalance(int user_id);
        int GameCount(int user_id);
    }
}
