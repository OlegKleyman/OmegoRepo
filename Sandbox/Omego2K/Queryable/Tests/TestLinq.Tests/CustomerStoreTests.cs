using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;

namespace TestLinq.Tests
{
    [TestFixture]
    public class CustomerStoreTests
    {

        [Test]
        public void GetAllTest()
        {
            var store = new Repository<Customer>(new CustomerDataProvider());
            var result = from x in store
                         where x.Name == "Oleg"
                         select x;
            var res = result.ToList();
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("Oleg", res[0].Name);

        }
    }
}
