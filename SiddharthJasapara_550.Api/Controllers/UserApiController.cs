using SiddharthJasapara_550.Model.Models;
using SiddharthJasapara_550.Repository.Repository;
using SiddharthJasapara_550.Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SiddharthJasapara_550.Api.Controllers
{
    public class UserApiController : ApiController
    {
        private readonly IUserInterface _UserServices;
        public UserApiController()
        {
            _UserServices = new UserServices();
        }

        [HttpPost]
        [Route("api/UserApi/Register")]
        public bool Register(UsersModel userModel)
        {
            return _UserServices.UserAdded(userModel);
        }

        [HttpPost]
        [Route("api/UserApi/Login")]
        public UsersModel Login(UserLoginModel loginModel)
        {
            return _UserServices.ExistingUser(loginModel);
        }

        [HttpGet]
        [Route("api/UserApi/Transactions")]
        public List<TransactionModel> Transactions(int user_id)
        {
            return _UserServices.GetTransactions(user_id);
        }

        [HttpGet]
        [Route("api/UserApi/GamesCount")]
        public int GamesCount(int user_id)
        {
            return _UserServices.GameCount(user_id);
        }

        [HttpPost]
        [Route("api/UserApi/Play")]
        public int Play(int user_id, int reward)
        {
            return _UserServices.GamePlay(reward, user_id);
        }

        [HttpGet]
        [Route("api/UserApi/GetBalance")]
        public int GetBalance(int user_id)
        {
            return _UserServices.GetBalance(user_id);
        }
    }
}