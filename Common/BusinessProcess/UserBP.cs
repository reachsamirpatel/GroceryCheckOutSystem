using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOutSystem.DataAccess;
using Logging;
using Utility;

namespace BusinessProcess
{
    public class UserBP
    {
        private List<Login> _loginList;
        private List<User> _userList;
        private Repository _repository;
        public User CurrentUser { get; set; }
        private ILogger _log;

        public UserBP()
        {
            Initialize();
        }

        private void Initialize()
        {
            _repository = new Repository();
            _userList = _repository.UserRepository.GetAll();
            _loginList = _repository.LoginRepository.GetAll();
            _log = LogManager.GetLogger(this);
        }

        public User CheckCredentials(string username, string password)
        {
            try
            {
                Login login =
                    _loginList.Find(
                        x =>
                            x.LoginName.ToLower() == username.ToLower() &&
                            Encryptor.Base64Decode(x.Password) == password);
                if (login == null)
                    return null;
                User user = _userList.FindLast(x => x.UserId == login.UserId);
                return user;
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Please Enter Login Credentials");
                    Console.WriteLine("Username");
                    string userName = Console.ReadLine()?.ToLowerInvariant();
                    Console.WriteLine("Password");
                    string password = ConsoleHelper.ReadLineMasked('*');
                    CurrentUser = CheckCredentials(userName, password);
                    string action = "f";
                    if (CurrentUser != null)
                    {
                        action = "s";
                        return;
                    }
                    switch (action)
                    {
                        case "s":
                            break;

                        default:
                            Console.WriteLine("Login Failed.");
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }

        }
    }
}
