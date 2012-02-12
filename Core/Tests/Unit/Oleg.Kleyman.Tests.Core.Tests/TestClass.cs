using System;

//test

namespace Oleg.Kleyman.Tests.Core.Tests
{
    public class TestClass : IComparable
    {
        public virtual int Baz { get; set; }
        public static int Qux { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        public virtual void Foo()
        {
        }

        public static void Bar()
        {
        }
    }
}