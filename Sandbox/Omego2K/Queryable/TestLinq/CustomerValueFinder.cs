using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestLinq
{
    public class CustomerValueFinder : ValueFinder
    {
        public CustomerValueFinder(Expression exp) : base(exp)
        {
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            return GetValue(node, "Name");
        }
    }
}
