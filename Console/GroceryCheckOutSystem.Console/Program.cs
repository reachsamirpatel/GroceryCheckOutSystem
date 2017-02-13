using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using Utility;
using System.IO;
using BusinessProcess;
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;

namespace GroceryCheckOutSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Check whether file required by the application is actually present or not
            if (!ValidationConsole.PerformHealthCheck())
                return;

            UserBP userBp = new UserBP();
            //Check login credentials
            userBp.Start();
            User currentUser = userBp.CurrentUser;
            if (currentUser.UserType == UserTypeEnum.Admin)
            {
                AdminConsole.Start(userBp);
            }
            else if (currentUser.UserType == UserTypeEnum.Marketing)
            {
                MarketingConsole.Start(userBp);
            }
            else if (currentUser.UserType == UserTypeEnum.Clerk)
            {
                ClerkConsole.Start(userBp);
            }
        }
    }
}
