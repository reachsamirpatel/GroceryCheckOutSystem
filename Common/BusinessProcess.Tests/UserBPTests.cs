using NUnit.Framework;
using BusinessProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;

namespace BusinessProcess.Tests
{
    [TestFixture()]
    public class UserBPTests
    {
        [Test()]
        public void UserBPTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CheckCredentialsTest()
        {
            UserBP userBp = new UserBP();
            List<User> userList = TestHelper.UserList.ToList();
            List<Login> loginList = TestHelper.LoginList.ToList();
            foreach (Login login in loginList)
            {
                User searchedUser = userList.FindLast(x => x.UserId == login.UserId);
                Assert.That(searchedUser.UserId == searchedUser.UserId);
            }
        }


    }
}