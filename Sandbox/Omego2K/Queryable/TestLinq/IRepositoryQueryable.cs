using System.Collections.Generic;
using System.Linq;

namespace TestLinq
{
    public interface IRepositoryQueryable<T> : IQueryable<T>
    {
        T GetResults(IEnumerable<string> values);
    }
}