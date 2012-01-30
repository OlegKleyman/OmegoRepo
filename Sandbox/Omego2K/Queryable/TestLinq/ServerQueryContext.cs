using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestLinq
{
    public class ServerQueryContext<T>
    {
        // Executes the expression tree that is passed to it.
        internal static object Execute<T>(Expression expression, bool IsEnumerable, DataProvider<T> provider)
        {
            // The expression must represent a query over the data source.
            if (!IsQueryOverDataSource(expression))
                throw new InvalidProgramException("No query over the data source was specified.");

            // Find the call to Where() and get the lambda expression predicate.
            InnermostWhereFinder whereFinder = new InnermostWhereFinder();
            MethodCallExpression whereExpression = whereFinder.GetInnermostWhere(expression);
            LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression)(whereExpression.Arguments[1])).Operand;

            // Send the lambda expression through the partial evaluator.
            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            // Get the place name(s) to query the Web service with.
            ValueFinder finder = new CustomerValueFinder(lambdaExpression.Body);
            List<string> values = finder.ExpressionValues;
            if (values.Count == 0)
            {
                throw new InvalidQueryException("You must specify at least one value in your query.");
            }
            
            var results = provider.GetData(values);

            // Copy the IEnumerable places to an IQueryable.
            IQueryable<T> customers = results.AsQueryable();

            // Copy the expression tree that was passed in, changing only the first
            // argument of the innermost MethodCallExpression.
            
            ExpressionTreeModifier<T> treeCopier = new ExpressionTreeModifier<T>(customers);
            Expression newExpressionTree = treeCopier.Visit(expression);

            // This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
            if (IsEnumerable)
                return customers.Provider.CreateQuery(newExpressionTree);
            else
                return customers.Provider.Execute(newExpressionTree);
        }

        private static bool IsQueryOverDataSource(Expression expression)
        {
            // If expression represents an unqueried IQueryable data source instance,
            // expression is of type ConstantExpression, not MethodCallExpression.
            return (expression is MethodCallExpression);
        }
    }

}
