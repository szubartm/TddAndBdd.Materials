using NUnit.Framework;

//ReSharper disable InconsistentNaming

namespace UnitTesting.Naming
{
    [TestFixture]
    public class CalculatorTest_PartTwo
    {
        [Test]
        public void TestForSimpleAdd()
        {
            //arrange
            var x = 10;
            var y = 0.5;
            var calculator = new Calculator();

            //act
            var result = calculator.Add(x, y);

            //assert
            Assert.That(result, Is.EqualTo(10.5));
        }
    }

    [TestFixture]
    public class Calculator_WhenTestingSimpleAdd
    {
        private double _result;

        //[SetUp]
        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            //arrange
            var x = 10;
            var y = 20.0;
            var objectUnderTest = new Calculator();

            //act
            _result = objectUnderTest.Add(x, y);
        }

        [Test]
        public void ShouldProduceCorrectResult() => Assert.That(_result, Is.EqualTo(30.0));

        [Test]
        public void WeCanHaveMultipleSeparatedTestsForResult() => Assert.Fail("Because we can...");//... but should we ???
    }

    [TestFixture]
    public class Calculator_WhenTestingCommutativeForAdd
    {
        private double _result1;
        private double _result2;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            //arrange
            var x = 10;
            var y = 20.0;
            var objectUnderTest = new Calculator();

            //act
            _result1 = objectUnderTest.Add(x, y);
            _result2 = objectUnderTest.Add((int)y, x);
        }

        [Test]
        public void ShouldProduceCorrectResults()
        {
            Assert.That(_result1, Is.EqualTo(30.0));
            Assert.That(_result2, Is.EqualTo(30.0));
        }

        [Test]
        public void ShouldBothResultsBeEqual() => Assert.That(_result1, Is.EqualTo(_result2));
    }

    //contextTest !!!
}