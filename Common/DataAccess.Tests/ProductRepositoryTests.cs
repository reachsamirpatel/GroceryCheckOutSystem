using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCheckOut.Entity;
using GroceryCheckOutSystem.DataAccess;
using NUnit.Framework;

namespace DataAccess.Tests
{
    [TestFixture()]
    public class ProductRepositoryTests
    {

        [Test()]
        public void GetAllProductTest()
        {
            ProductRepository productRepository = new ProductRepository();
            List<Product> productList = productRepository.GetAll();
            List<Product> expectedList = TestHelper.TestProductsOrignal.ToList();
            foreach (Product product in productList)
            {
                Product output = expectedList.Find(x => x.Name == product.Name);
                Assert.That(output != null);
            }
        }

        [Test()]
        public void UpSertProductTest()
        {
            ProductRepository productRepository = new ProductRepository();
            Product product = new Product("xyz", 7);
            List<Product> productList = productRepository.GetAll();
            int productListCount = productList.Count;
            productList.Add(product);
            productRepository.UpSert(productList);
            productList = productRepository.GetAll();
            Assert.That(productListCount + 1 == productList.Count);
            productList.RemoveAt(productList.Count - 1);
            productRepository.UpSert(productList);
        }
    }
}