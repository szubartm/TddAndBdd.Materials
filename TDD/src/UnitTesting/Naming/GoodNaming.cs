using System;

using NUnit.Framework;

namespace UnitTesting.Naming
{
    [TestFixture]
    public class CalculatorTest
    {
        Calculator _objectUnderTest;

        [SetUp]
        public void CreateCalculator() => _objectUnderTest = new Calculator();

        [Test]
        //bad practice from the past [ExpectedException( typeof ( ArgumentException ) )]
        public void ShouldThrowExceptionForNegativeFirstParameter() => Assert.Throws<ArgumentOutOfRangeException>(() => _objectUnderTest.Add(-1, 0));

        [Test]
        public void ShouldIntegersBeCommutativeUnderAddition()
        {
            var x = 10;
            var y = 20;
            var result1 = _objectUnderTest.Add(x, y);
            var result2 = _objectUnderTest.Add(y, x);
            //Assert.IsTrue(result1 == result2);
            Assert.That(result1, Is.EqualTo(result2).Within(double.Epsilon));
        }

        [TestCase(1, -0.1, ExpectedResult = 0.9)]
        [TestCase(5, 10, ExpectedResult = 15)]
        public double Should_return_correct_values_for_example_data(int x, double y) 
            => _objectUnderTest.Add(x, y);
    }
}