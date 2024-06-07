using Newtonsoft.Json;
using SiddharthJasapara_550.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.CustomFilters;

namespace SiddharthJasapara_550.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string jsonContent = JsonConvert.SerializeObject(loginModel);
                    string url = "api/UserApi/Login";
                    string response = await WebApiHelper.HttpClientPostResponse(url, jsonContent);

                    UsersModel existingUser = JsonConvert.DeserializeObject<UsersModel>(response);
                    if (existingUser.id != 0)
                    {
                        TempData["LoggedIn"] = "Logged In";
                        SessionHelper.Id = existingUser.id;
                        SessionHelper.Username = existingUser.username;
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        ViewBag.error = "Invalid Credentials";
                        return View();
                    }
                }
                ViewBag.empty = "Please Fill the details";
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UsersModel userModel)
        {
            try
            {
                userModel.id = 0;
                if (ModelState.IsValid)
                {
                    string jsonContent = JsonConvert.SerializeObject(userModel);
                    string url = "api/UserApi/Register";
                    string response = await WebApiHelper.HttpClientPostResponse(url, jsonContent);

                    bool added = JsonConvert.DeserializeObject<bool>(response);
                    if (added)
                    {
                        TempData["Registered"] = "User Registered";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.error = "User Already Registered";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [RoleFilter]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [RoleFilter]
        [HttpGet]
        public async Task<ActionResult> Wallet(int? current)
        {
            try
            {
                string url = "api/UserApi/Transactions?user_id=" + SessionHelper.Id;
                string response = await WebApiHelper.HttpClientRequestResponse(url);

                List<TransactionModel> transactions = JsonConvert.DeserializeObject<List<TransactionModel>>(response);

                int maxRows = 5;
                int currentIndex = current ?? 1;
                List<TransactionModel> list = transactions.Skip((currentIndex - 1) * maxRows).Take(maxRows).ToList();
                PaginationModel paginationModel = new PaginationModel();
                paginationModel.currentIndex = currentIndex;
                paginationModel.transactionsModel = list;
                paginationModel.total = transactions.Count();

                ViewBag.count = paginationModel.total;
                ViewBag.currentIndex = paginationModel.currentIndex;

                ViewBag.transactionsModel = paginationModel.transactionsModel;

                DateTime now = DateTime.Now;
                DateTime fromDate = DateTime.Now.AddDays(-1);
                List<TransactionModel> transactionList = transactions.Where(t => t.date >= fromDate && t.date <= now).ToList();
                TransactionModel transaction = transactions[paginationModel.total - 1];
                if (transaction.date >= fromDate && transaction.date <= now)
                {
                    transactionList.Remove(transaction);
                }

                int earnings = 0;
                foreach (TransactionModel t in transactionList)
                {
                    if (t.credited == null)
                    {
                        t.credited = 0;
                    }
                    earnings += t.credited;
                }
                TempData["earnings"] = earnings;
                TempData["Balance"] = transactions[0].balance;

                return View(new List<PaginationModel> { paginationModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [RoleFilter]
        [HttpGet]
        public async Task<ActionResult> Games()
        {
            try
            {
                string url = "api/UserApi/GetBalance?user_id=" + SessionHelper.Id;
                string response = await WebApiHelper.HttpClientRequestResponse(url);

                int balance = JsonConvert.DeserializeObject<int>(response);
                if (balance < 20)
                {
                    TempData["balance"] = balance;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [RoleFilter]
        [HttpGet]
        public async Task<JsonResult> GamesCount()
        {
            try
            {
                string url = "api/UserApi/GamesCount?user_id=" + SessionHelper.Id;
                string response = await WebApiHelper.HttpClientRequestResponse(url);

                int games = JsonConvert.DeserializeObject<int>(response);
                return Json(games, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [RoleFilter]
        [HttpPost]
        public async Task<JsonResult> Games(int reward)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(reward);
                string url = "api/UserApi/Play?user_id=" + SessionHelper.Id + "&reward=" + reward;
                string response = await WebApiHelper.HttpClientPostResponse(url, jsonContent);

                int balance = JsonConvert.DeserializeObject<int>(response);
                return Json(balance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [RoleFilter]
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LoggedOut"] = "Session Ended!!";
            return RedirectToAction("Login");
        }
    }
}