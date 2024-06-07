
using SiddharthJasapara_550.Helper.Helpers;
using SiddharthJasapara_550.Model.DBContext;
using SiddharthJasapara_550.Model.Models;
using SiddharthJasapara_550.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiddharthJasapara_550.Repository.Service
{
    public class UserServices : IUserInterface
    {
        private RewardGame550Entities DBContext = new RewardGame550Entities();

        public bool UserAdded(UsersModel userModel)
        {
            try
            {
                Users user = UserHelper.CustomToDB(userModel);
                if (user != null)
                {
                    Users user1 = DBContext.Users.FirstOrDefault(u => u.email == user.email);
                    if (user1 != null)
                    {
                        return false;
                    }
                    DBContext.Users.Add(user);
                    DBContext.SaveChanges();

                    bool rewarded = AddWelcomeReward(user);

                    if (rewarded)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsersModel ExistingUser(UserLoginModel loginModel)
        {
            try
            {
                Users user = DBContext.Users.FirstOrDefault(u => u.email == loginModel.email && u.password == loginModel.password);
                UsersModel userModel = new UsersModel();
                if (user != null)
                {
                    userModel = UserHelper.DBToCustom(user);
                }
                return userModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddWelcomeReward(Users user)
        {
            try
            {
                Transactions transaction = new Transactions();
                transaction.user_id = user.id;
                transaction.credited = 100;
                transaction.date = DateTime.Now;
                transaction.balance = (int)(0 + transaction.credited);

                DBContext.Transactions.Add(transaction);
                DBContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TransactionModel> GetTransactions(int user_id)
        {
            try
            {
                List<TransactionModel> list = new List<TransactionModel>();
                if (user_id != 0)
                {
                    List<Transactions> transactions = DBContext.Transactions.Where(t => t.user_id == user_id).OrderByDescending(t => t.id).ToList();
                    if (transactions.Count != 0)
                    {
                        foreach (Transactions t in transactions)
                        {
                            TransactionModel transactionModel = UserHelper.DBToCustom(t);
                            list.Add(transactionModel);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetBalance(int user_id)
        {
            try
            {
                int balance = 0;
                if (user_id != 0)
                {
                    List<Transactions> transactions = DBContext.Transactions.Where(t => t.user_id == user_id).OrderByDescending(t => t.id).ToList();
                    if (transactions.Count != 0)
                    {
                        balance = transactions[0].balance;
                    }
                }
                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GamePlay(int reward, int user_id)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime fromDate = now.AddDays(-1);
                Transactions first = DBContext.Transactions.Where(t => t.user_id == user_id).OrderBy(t => t.id).FirstOrDefault();
                List<Transactions> list = DBContext.Transactions.Where(t => t.date <= now && t.date >= fromDate && t.user_id == user_id).ToList();
                if (first.date <= now && first.date >= fromDate)
                {
                    list.Remove(first);
                }
                List<Game> games = DBContext.Game.Where(g => g.user_id == user_id).ToList();

                int total = 0;
                Transactions last = (from Transactions in DBContext.Transactions where Transactions.user_id == user_id orderby Transactions.date descending select Transactions).FirstOrDefault();

                foreach (Transactions t in list)
                {
                    if (t.credited == null)
                    {
                        t.credited = 0;
                    }
                    total += (int)t.credited;
                }

                if (last.balance - 20 > 0)
                {
                    Game game = new Game();
                    game.user_id = user_id;
                    game.number = reward;
                    DBContext.Game.Add(game);
                    DBContext.SaveChanges();

                    Transactions t = new Transactions();
                    if (games.Count >= 3)
                    {
                        t.user_id = user_id;
                        t.debited = 20;
                        t.date = DateTime.Now;
                        t.balance = last.balance - 20;

                        DBContext.Transactions.Add(t);
                        DBContext.SaveChanges();
                    }

                    last = (from Transactions in DBContext.Transactions where Transactions.user_id == user_id orderby Transactions.date descending select Transactions).FirstOrDefault();

                    if (total + reward <= 500)
                    {
                        Transactions transaction = new Transactions();
                        transaction.user_id = user_id;
                        transaction.credited = reward;
                        transaction.date = DateTime.Now;
                        transaction.balance = last.balance + reward;

                        DBContext.Transactions.Add(transaction);
                        DBContext.SaveChanges();

                        //last = (from Transactions in DBContext.Transactions where Transactions.user_id == user_id orderby Transactions.date descending select Transactions).FirstOrDefault();
                        //if(last.balance == 500)
                        //{
                        //    return 500;
                        //}
                        return reward;
                    }
                    return 0;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GameCount(int user_id)
        {
            List<Game> games = DBContext.Game.Where(g => g.user_id == user_id).ToList();
            return games.Count;
        }
    }
}
