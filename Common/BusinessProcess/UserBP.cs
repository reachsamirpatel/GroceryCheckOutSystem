using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOutSystem.DataAccess;
using Utility;

namespace BusinessProcess
{
    public class UserBP
    {
        private List<Login> _loginList;
        private List<User> _userList;
        private Repository _repository;
        public User CurrentUser { get; set; }

        public UserBP()
        {
            Initialize();
        }

        private void Initialize()
        {
            _repository = new Repository();
            _userList = _repository.UserRepository.GetAll();
            _loginList = _repository.LoginRepository.GetAll();
        }

        public User CheckCredentials(string username, string password)
        {
            Login login = _loginList.FindLast(x => x.LoginName.ToLower() == username.ToLower() && Encryptor.Base64Decode(x.Password) == password);
            if (login == null)
                return null;
            User user = _userList.FindLast(x => x.UserId == login.UserId);
            return user;
        }
        public string ReadLineMasked(char mask = '*')
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write(mask);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);

                    if (Console.CursorLeft == 0)
                    {
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                    }
                    else Console.Write("\b \b");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Please Enter Login Credentials");
                Console.WriteLine("Username");
                string userName = Console.ReadLine()?.ToLowerInvariant();
                Console.WriteLine("Password");
                string password = ReadLineMasked();
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
    }
}
