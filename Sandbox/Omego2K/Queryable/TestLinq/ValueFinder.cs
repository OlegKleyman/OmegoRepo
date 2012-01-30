using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestLinq
{
    public abstract class ValueFinder : ExpressionVisitor
    {
        private Expression expression;
        private List<string> Values;

        public ValueFinder(Expression exp)
        {
            this.expression = exp;
            Values = new List<string>();
        }

        public List<string> ExpressionValues
        {
            get
            {
                Values = new List<string>();
                Visit(expression);

                return Values;
            }
        }

        protected Expression GetValue(BinaryExpression be, string propertyName)
        {
            if (be.NodeType == ExpressionType.Equal)
            {
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(be, typeof(Customer), propertyName))
                {
                    Values.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression(be, typeof(Customer), propertyName));
                    return be;
                }
            }

            return base.VisitBinary(be);
        }
        
    }

}
