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
    public class ReceiptBPTests
    {

        [Test()]
        public void CreateReceiptTest()
        {
            ReceiptBP receiptBp = new ReceiptBP();
            string receipt = receiptBp.CreateReceipt(TestHelper.Basket1.ToList(), TestHelper.UserList.FirstOrDefault());
            Assert.That(receipt != null);
        }
    }
}