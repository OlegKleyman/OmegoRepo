using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TestLinq
{
    public abstract class DataProvider<T> : IQueryProvider
    {
        #region Implementation of IQueryProvider

        public IQueryable CreateQuery(Expression expression)
        {
            Debug.WriteLine("inside!!!!!!!!!!!!");
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Repository<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Repository<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)ServerQueryContext<TResult>.Execute(expression, isEnumerable, this);
        }

        #endregion

        public abstract T[] GetData(IEnumerable<string> values);
    }
}
