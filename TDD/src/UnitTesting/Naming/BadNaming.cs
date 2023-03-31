using System;

using NUnit.Framework;

namespace UnitTesting.Naming
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    //Bad name on purpose !!!
    public class _Calculator
    {
        [Test]
        public void Test1()
        {
            //Bad practice on purpose !!!
            Assert.IsTrue(new Calculator().Add(12, 13) == 25);
        }

        [Test]
        public void Test2()
        {
            var c = new Calculator();
            var x = c.Add(12, 13);
            var y = c.Add(13, 12);
            var z = c.Add(1, 0.5);
            Assert.IsNotNull(x);
            //Bad practice on purpose !!!
            Assert.IsTrue(x == y);
            Assert.AreEqual(25, x);
            Assert.That(1.5, Is.EqualTo(z));
        }

        [Test]
        public void TestException()
        {
            try
            {
                new Calculator().Add(-1, -2);
                Assert.Fail();//Bad practice on purpose !!!
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }
}
