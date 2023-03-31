using FakeItEasy;
using Moq;

namespace Common.TestUtils
{
    public abstract class ContextTestUsingFakeItEasy<T> : ContextTest<T> where T: class
    {
        protected TStub Stub<TStub>() where TStub : class 
            => A.Fake<TStub>();
    }
    
    public abstract class ContextTestUsingMoq<T> : ContextTest<T> where T: class
    {
        protected TStub Stub<TStub>() where TStub : class 
            => new Mock<TStub>().Object;
    }
}
