using System;
using NUnit.Framework;

namespace Common.TestUtils
{
    /// <summary> The AAA principle testing class used as a base class for all our tests </summary>
    public abstract class ContextTest<T> where T : class
    {
        protected T ObjectUnderTest;

        /// <summary> The setup of the test </summary>
        [OneTimeSetUp]
        public void TestSetup()
        {
            ObjectUnderTest = Arrange();
            Act();
        }

        /// <summary>Arranges the properties and context for object under test</summary>
        protected abstract T Arrange();

        /// <summary>The object under test is called within this method.</summary>
        protected abstract void Act();

        /// <summary> Runs the action on object under test, returns an exception if thrown. In most cases a <see cref="Assert.Throws{TActual}(TestDelegate,string?,object?[])"/> provides you with better experience</summary>
        /// <param name="action">The action on object under test.</param>
        protected Exception Trying(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
}