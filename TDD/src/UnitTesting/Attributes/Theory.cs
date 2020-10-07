using System;
using NUnit.Framework;

namespace UnitTesting.Attributes
{
    public class SqrtTests
    {
        [Datapoint]
        public double Zero = 0;

        [Datapoint]
        public double Positive = 1;

        [Datapoint]
        public double Negative = -1;

        [Datapoint]
        public double Max = double.MaxValue;

        [Datapoint]
        public double Infinity = double.PositiveInfinity;

        [Theory]
        public void SquareRootDefinition(double num)
        {
            Assume.That(num >= 0.0 && num < double.MaxValue);

            double sqrt = Math.Sqrt(num);

            Assert.That(sqrt >= 0.0);
            Assert.That(sqrt * sqrt, Is.EqualTo(num).Within(0.000001));
        }
    }
}
