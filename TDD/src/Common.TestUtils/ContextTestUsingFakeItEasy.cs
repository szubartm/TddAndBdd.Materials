using FakeItEasy;

namespace Common.TestUtils
{
    /// <summary>
    /// The AAA principle testing class used as a base class for all our tests
    /// </summary>
    public abstract class ContextTestUsingFakeItEasy<T> : ContextTest<T> where T: class
    {
        protected TStub Stub<TStub>() where TStub : class 
            => A.Fake<TStub>();
    }
}
