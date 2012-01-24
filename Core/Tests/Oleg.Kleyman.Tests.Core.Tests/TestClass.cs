using System;

namespace Oleg.Kleyman.Tests.Core.Tests
{
    public class TestClass : IComparable
    {
        public void Foo(){}
        public static void Bar(){}
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int Baz { get; set; }
        public static int Qux { get; set; }
    }
}