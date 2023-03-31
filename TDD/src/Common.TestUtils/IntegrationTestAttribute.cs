using System;
using NUnit.Framework;

namespace Common.TestUtils
{
    /// <summary>
    /// Attribute to decorate a test fixture or single test, marking it as integration (i.e. long running) test rather than unit test (i.e. fast running test)
    /// Intended usage: any tests which actually are long running should be also decorated with this attribute so in the future we can run in CI nunit.exe only for these tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IntegrationTestAttribute : CategoryAttribute
    {
        public IntegrationTestAttribute() : base("Integration") { }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class LongRunningTestAttribute : CategoryAttribute
    {
        public LongRunningTestAttribute() : base("LongRunning") { }
    }
}
