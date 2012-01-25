namespace Oleg.Kleyman.Tests.Core.Tests
{
    public class TestClassTwo : TestClass
    {
        public int Quux { get; set; }
        public override int Baz
        {
            get
            {
                return base.Baz;
            }
            set
            {
                base.Baz = value;
            }
        }
        public override void Foo()
        {
            base.Foo();
        }
    }
}