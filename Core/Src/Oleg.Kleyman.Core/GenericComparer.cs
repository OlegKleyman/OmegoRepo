using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Core
{
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _compareHandler;

        public GenericComparer(Func<T, T, bool> compareHandler)
        {
            _compareHandler = compareHandler;
        }

        public bool Equals(T x, T y)
        {
            return _compareHandler(x, y);
        }

        public int GetHashCode(T target)
        {
            return target.GetHashCode();
        }
    }
}
