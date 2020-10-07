using System;
using NUnit.Framework;

namespace UnitTesting.Naming
{
    [TestFixture]
    public class _Calculator
    {
        [Test]
        public void Test1()
        {
            Assert.IsTrue(new Calculator().Add( 12,13 ) == 25);
        }

        [Test]
        public void Test2()
        {
            var c = new Calculator();
            var x = c.Add( 12, 13 );
            var y = c.Add( 13, 12 );
            var z = c.Add( 1, 0.5 );
            Assert.IsNotNull( x );
            Assert.IsTrue( x == y );
            Assert.AreEqual( 25, x );
            Assert.That( 1.5, Is.EqualTo( z ) );
        }

        [Test]
        public void TestException()
        {
            try
            {
                new Calculator().Add( -1, -2 );
                Assert.Fail();
            }
            catch ( Exception ex)
            {
                Assert.IsNotNull( ex );
            }
        }
    }
}
