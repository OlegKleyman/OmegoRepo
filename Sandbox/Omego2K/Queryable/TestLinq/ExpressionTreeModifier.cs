using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestLinq
{
    public class ExpressionTreeModifier<T> : ExpressionVisitor
    {
        private IQueryable queryable;

        internal ExpressionTreeModifier(IQueryable customers)
        {
            this.queryable = customers;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Type == typeof(Repository<Customer>))
                return Expression.Constant(this.queryable);
            else
                return c;
        }
    }

}
