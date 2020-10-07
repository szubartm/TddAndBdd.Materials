using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace UnitTesting.Attributes
{
    [TestFixture]
    public class ExpectedExceptionTests
    {
        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestArgumentOutOfRangeException() => ExceptionThrowingMethod();

        [Test, ExpectedException(typeof(ArgumentException), true)]
        public void TestArgumentException() => ExceptionThrowingMethod();

        [Test, ExpectedException(typeof(Exception), true)]
        public void TestException() => ExceptionThrowingMethod();

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Message")]
        public void TestExpectedMessage() => ExceptionThrowingMethod();

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = null)]
        public void TestExpectedMessageNull() => ExceptionThrowingMethod();

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Bad message")]
        [Explicit]
        public void TestExpectedMessageFail() => ExceptionThrowingMethod();

        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        private static void ExceptionThrowingMethod()
        {
            bool b = true;
            if (b || !b)
                throw new ArgumentOutOfRangeException("", @"Message");

            Assert.That(0, Is.EqualTo(1));
            Assert.Fail("Exception not caught");
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExpectedExceptionAttribute : NUnitAttribute, IWrapTestMethod
    {
        private readonly Type _expectedExceptionType;
        public bool AllowInheritedExceptions { get; set; }
        public string ExpectedMessage { get; set; }

        public ExpectedExceptionAttribute(Type expectedExceptionType, bool allowInheritedExceptions = false, string expectedMessage = null)
        {
            _expectedExceptionType = expectedExceptionType;
            AllowInheritedExceptions = allowInheritedExceptions;
            ExpectedMessage = expectedMessage;
        }

        public TestCommand Wrap(TestCommand command) => new ExpectedExceptionCommand(command, _expectedExceptionType, AllowInheritedExceptions, ExpectedMessage);

        private class ExpectedExceptionCommand : DelegatingTestCommand
        {
            private readonly Type _expectedType;
            private readonly bool _allowInheritedExceptions;
            private readonly string _expectedMessage;

            public ExpectedExceptionCommand(TestCommand innerCommand, Type expectedType, bool allowInheritedExceptions, string expectedMessage)
                : base(innerCommand)
            {
                _expectedType = expectedType;
                _allowInheritedExceptions = allowInheritedExceptions;
                _expectedMessage = expectedMessage;
            }

            public override TestResult Execute(TestExecutionContext context)
            {
                Type caughtType = null;
                string message = null;

                try
                {
                    innerCommand.Execute(context);
                }
                catch (Exception ex)
                {
                    if (ex is NUnitException && ex.InnerException != null)
                        ex = ex.InnerException;
                    caughtType = ex.GetType();
                    message = ex.Message;
                }
                if (caughtType == null)
                    context.CurrentResult.SetResult(ResultState.Failure, $"Expected {_expectedType.Name}{(_allowInheritedExceptions ? " or it's subtypes " : "")}but no exception was thrown");
                else
                {
                    var errors = new List<string>();

                    if (_allowInheritedExceptions)
                    {
                        if (!_expectedType.IsAssignableFrom(caughtType))
                            errors.Add($"Expected {_expectedType.Name} or it's subtype but got {caughtType.Name}");
                    }
                    else
                    {
                        if (caughtType != _expectedType)
                            errors.Add($"Expected {_expectedType.Name} but got {caughtType.Name}");
                    }
                    if (_expectedMessage != null && _expectedMessage != message)
                        errors.Add($"Expected message was '{_expectedMessage}' but got '{message}'");

                    if (errors.Count == 0)
                        context.CurrentResult.SetResult(ResultState.Success);
                    else
                        context.CurrentResult.SetResult(ResultState.Failure, string.Join(Environment.NewLine, errors));
                }

                return context.CurrentResult;
            }
        }
    }
}