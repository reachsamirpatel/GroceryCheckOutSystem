using NUnit.Framework;
using GroceryCheckOutSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using Utility;

namespace DataAccess.Tests
{
    [TestFixture()]
    public class LoginRepositoryTests
    {


        [Test()]
        public void GetAllLoginTest()
        {
            LoginRepository loginRepository = new LoginRepository();
            List<Login> loginList = loginRepository.GetAll();
            List<Login> expectedList = TestHelper.LoginList.ToList();
            foreach (Login login in loginList)
            {
                Login output = expectedList.Find(x => x.UserId == login.UserId);
                Assert.That(output != null);
            }
        }

        [Test()]
        public void UpSertLoginTest()
        {
            LoginRepository loginRepository = new LoginRepository();
            Login login = new Login("testingUser", Encryptor.Base64Encode("xyz"), Guid.NewGuid());
            List<Login> loginList = loginRepository.GetAll();
            int loginListCount = loginList.Count;
            loginList.Add(login);
            loginRepository.UpSert(loginList);
            loginList = loginRepository.GetAll();
            Assert.That(loginListCount + 1 == loginList.Count);
            loginList.RemoveAt(loginList.Count - 1);
            loginRepository.UpSert(loginList);
        }
    }
}