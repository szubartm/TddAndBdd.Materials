using System.Diagnostics;
using NUnit.Framework;

namespace UnitTesting.Attributes
{
    [TestFixture]
    public class AttributesLifecycleExample
    {
        [OneTimeSetUp]
        public void CalledOncePerFixtureBeforeAnyTests() => Debug.WriteLine("CalledOncePerFixture - before any tests");

        [SetUp]
        public void CalledBeforeEachTest() => Debug.WriteLine( "CalledBeforeEachTest" );

        [Test]
        public void SomeTestMethod()
        {
            Debug.WriteLine("Test"  );
            Assert.IsTrue( true );
        }

        [TestCase(10,20, ExpectedResult = 30)]
        [TestCase(-10, -20, ExpectedResult = -30)]
        public int CalculationTestMethod( int x, int y )
        {
            Debug.WriteLine( "CalculationTest: {0}, {1}", x, y );
            return x + y;
        }

        [TearDown]
        public void CalledAfterEachTest() => Debug.WriteLine("CalledAfterEachTest");

        [OneTimeTearDown]
        public void CalledOncePerFixtureAfterAllTests() => Debug.WriteLine("CalledOncePerFixture - after all tests");
    }
}
