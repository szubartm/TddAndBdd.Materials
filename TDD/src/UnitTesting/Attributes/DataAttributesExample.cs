using System;
using System.Threading;
using NUnit.Framework;

namespace UnitTesting.Attributes
{
    [TestFixture]
    class DataAttributesExample
    {
        private Calculator _out;

        [OneTimeSetUp]
        public void CreateObjectUnderTest() => _out = new Calculator();

        [TestCase(1, 0.5, 1.5)]
        [TestCase(2, 0.01, 2.01)]
        public void TestCalculator(int x, double y, double expectedResult)
        {
            var actualValue = _out.Add(x, y);
            Assert.That(actualValue, Is.EqualTo(expectedResult));
        }

        [TestCase(1, 0.5, ExpectedResult = 1.5)]
        [TestCase(2, 0.01, ExpectedResult = 2.01)]
        public double TestCalculator2(int x, double y) => _out.Add(x, y);

        [Test]
        //ERROR ? [ExpectedException( typeof ( ArgumentException ), ExpectedMessage = "x" )]
        public void ExceptionHandlingExample() 
            => Assert.That(() => _out.Add(-1, 1.65), Throws.ArgumentException.And.Message.EqualTo("x"));

        [Test]
        [Timeout(2000)]
        public void RunLongOperation() => Thread.Sleep(2500);

        [Test]
        public void YetAnotherExample([Values(1, 2, 3)] int x, [Range(0.1, 0.6, 0.1)] double y)
        {
            var actualValue = _out.Add(x, y);
            Assert.That(actualValue, Is.GreaterThan(x));
        }
    }
}