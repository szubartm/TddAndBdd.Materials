using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

//[assembly: UnitTesting.Attributes.ConsoleAction("ASSEMBLY")]
namespace UnitTesting.Attributes
{
    [TestFixture]
    [ConsoleAction("CLASS")]
    public class ActionAttributeSampleTests
    {
        [Test]
        [ConsoleAction("METHOD")]
        [TestCase("02")]
        [TestCase("01")]
        public void SimpleTest(string number) => Console.WriteLine(@"Test run {0}.", number);
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class |
                 AttributeTargets.Interface | AttributeTargets.Assembly,
                 AllowMultiple = true)]
    public class ConsoleActionAttribute : Attribute, ITestAction
    {
        private readonly string _message;

        public ConsoleActionAttribute(string message) { _message = message; }

        public void BeforeTest(ITest test) => WriteToConsole("Before", test);

        public void AfterTest(ITest test) => WriteToConsole("After", test);

        public ActionTargets Targets => ActionTargets.Test | ActionTargets.Suite;

        private void WriteToConsole(string eventMessage, ITest details)
        {
            Console.WriteLine(
                $@"{eventMessage} {(details.IsSuite ? "Suite" : "Case")}: {_message}, from {(details.Fixture != null ? details.ClassName : "{no fixture}")}.{(details.Method != null ? details.Method.Name : "{no method}")}."
            );
        }
    }
}
