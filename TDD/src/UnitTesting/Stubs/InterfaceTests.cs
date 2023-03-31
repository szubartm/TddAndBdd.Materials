using System;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    [TestFixture(1, "Ala", TypeArgs = new[] { typeof(NewOrderObject) })]
    //[TestFixture(new object[] { typeof(NewOrderObject), 1, "Ala"})]
    [TestFixture(2, "Mike", TypeArgs = new[] { typeof(ReplaceOrderObject) })]
    [TestFixture(3, "Peter", TypeArgs = new[] { typeof(FillOrderObject) })]
    sealed class InterfaceTests<TInsertionObject> where TInsertionObject : IInsertionObject
    {
        private readonly int _i;
        private readonly string _name;

        private TInsertionObject _io;

        public InterfaceTests(int i, string name)
        {
            _i = i;
            _name = name;
        }

        [OneTimeSetUp]
        public void Setup() => _io = Activator.CreateInstance<TInsertionObject>();

        [Test]
        public void GetContent_ShouldReturnDifferentValues() => Console.WriteLine(_io.GetContent());

        [Test]
        public void PrintClassParams() => Console.WriteLine($@"{nameof(_i)}: {_i}, {nameof(_name)}: {_name}");
    }

    interface IInsertionObject
    {
        string GetContent();
    }

    class NewOrderObject : IInsertionObject
    {
        public string GetContent() => "NEW";
    }
    class ReplaceOrderObject : IInsertionObject
    {
        public string GetContent() => "REPLACE";
    }
    class FillOrderObject : IInsertionObject
    {
        public string GetContent() => "FILL";
    }
}