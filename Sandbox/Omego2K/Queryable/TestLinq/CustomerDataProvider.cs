using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestLinq
{
    public class CustomerDataProvider : DataProvider<Customer>
    {
        private Customer[] Customers = new[]
                                           {
                                               new Customer
                                                   {
                                                       Name = "Oleg",
                                                       Age = 26
                                                   },
                                               new Customer
                                                   {
                                                       Name = "James",
                                                       Age = 81
                                                   },
                                               new Customer
                                                   {
                                                       Name = "Ged",
                                                       Age = 56
                                                   }
                                           };
        #region Overrides of DataProvider<Customer>

        public override Customer[] GetData(IEnumerable<string> values)
        {
            var customers = from x in Customers
                            where values.Contains(x.Name)
                            select x;
            return customers.ToArray();
        }

        #endregion
    }
}
