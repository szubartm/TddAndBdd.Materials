using FakeItEasy;
using Moq;

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
    
    /// <summary>
    /// The AAA principle testing class used as a base class for all our tests
    /// </summary>
    public abstract class ContextTestUsingMoq<T> : ContextTest<T> where T: class
    {
        protected TStub Stub<TStub>() where TStub : class 
            => new Mock<TStub>().Object;
    }
}
