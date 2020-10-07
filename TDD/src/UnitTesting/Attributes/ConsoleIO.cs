using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace UnitTesting.Attributes
{
    [TestFixture]
    public class ConsoleInputAttributeTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [ConsoleInput("ABC")]
        public void Input_SimpleMessageTest(int i)
        {
            var @in = Console.ReadLine();
            Assert.That(@in, Is.EqualTo("ABC"));
        }


        private static readonly string _inMessage = "FieldTestMessage";
        [Test, ConsoleInputSource(nameof(_inMessage))]
        public void Input_MessageSourceFieldTest()
        {
            var @in = Console.ReadLine();
            Assert.That(@in, Is.EqualTo(_inMessage));
        }


        private string InMessage => "PropertyTestMessage";
        [Test, ConsoleInputSource(nameof(InMessage))]
        public void Input_MessageSourcePropertyTest()
        {
            var @in = Console.ReadLine();
            Assert.That(@in, Is.EqualTo(InMessage));
        }


        private static string GetInMessage() => "MethodTestMessage";
        [Test, ConsoleInputSource(nameof(GetInMessage))]
        public void Input_MessageSourceMethodTest()
        {
            var @in = Console.ReadLine();
            Assert.That(@in, Is.EqualTo(GetInMessage()));
        }



        private static string GetOutMessage() => "ABC123";
        [Test, ConsoleOutputSource(nameof(GetOutMessage))]
        public void Output_MessageSourceMethodTest() => Console.Write(GetOutMessage());

        [Test, ConsoleOutput("QWE123")]
        public void Output_Standard() => Console.Write(@"QWE123");

        [Test, ConsoleOutputStartsWith("ABC")]
        public void Output_StartsWith() => Console.Write(@"ABC123456");

        [Test, ConsoleOutputEndsWith("456")]
        public void Output_EndsWith() => Console.Write(@"ABC123456");

        [Test, ConsoleOutputRegex(@"^A\w{1,2}1\d{4}6$")]
        public void Output_RegEx() => Console.Write(@"ABC123456");

        [Test, ConsoleOutputRegex(@"A\w{1,2}1\d{4}6")]
        public void Output_RegExPartial() => Console.Write(@"XXX_ABC123456_XXX");


        private static readonly string _pattern = /*language=regexp|jsregexp*/@"^A\w{1,2}1\d{4}99$";
        [Test, ConsoleOutputSource(nameof(_pattern), MatchingType.Regex)]
        public void Output_RegExFromSource() => Console.Write(@"ABC1234599");

        // ReSharper disable once UnusedMember.Local
        private static string MethodThatAllowsPatternFieldToExist() => _pattern + "55";
    }

    public enum MatchingType : byte { Standard, Regex, StartsWith, EndsWith }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class ConsoleInputOutputBaseAttribute : NUnitAttribute, IWrapTestMethod
    {
        public virtual TestCommand Wrap(TestCommand command) => null;

        protected static string GetText(Type type, string memberName)
        {
            var member = type.GetMember(memberName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault();

            switch (member)
            {
                case MethodInfo methodInfo:
                    object instance1 = methodInfo.IsStatic ? null : Activator.CreateInstance(type);
                    return methodInfo.Invoke(instance1, null)?.ToString();
                case FieldInfo fieldInfo:
                    object instance2 = fieldInfo.IsStatic ? null : Activator.CreateInstance(type);
                    return fieldInfo.GetValue(instance2)?.ToString();
                case PropertyInfo propertyInfo:
                    object instance3 =
                        (propertyInfo.GetMethod?.IsStatic ?? false) || (propertyInfo.SetMethod?.IsStatic ?? false)
                            ? null
                            : Activator.CreateInstance(type);
                    return propertyInfo.GetValue(instance3, null)?.ToString();
                //case null:
                default:
                    throw new ArgumentOutOfRangeException(nameof(memberName), $@"Member {type.Name}.{nameof(memberName)} does not exist or it's type is not supported");
            }
        }

        protected sealed class ConsoleInCommand : DelegatingTestCommand
        {
            private readonly string _message;

            public ConsoleInCommand(TestCommand innerCommand, string message) : base(innerCommand) => _message = message;

            public override TestResult Execute(TestExecutionContext context)
            {
                var originalReader = Console.In;
                try
                {
                    using (var reader = new StringReader(_message))
                    {
                        Console.SetIn(reader);
                        innerCommand.Execute(context);
                    }

                    context.CurrentResult.SetResult(ResultState.Success);
                }
                catch (Exception ex)
                {
                    context.CurrentResult.SetResult(ResultState.Error, $"Exception occured: {ex}");
                }
                finally
                {
                    Console.SetIn(originalReader);
                }
                return context.CurrentResult;
            }
        }

        protected sealed class ConsoleOutCommand : DelegatingTestCommand
        {
            private readonly string _expectedMessage;
            private readonly MatchingType _matchingType;

            public ConsoleOutCommand(TestCommand innerCommand, string expectedMessage, MatchingType matchingType) : base(innerCommand) { _expectedMessage = expectedMessage; _matchingType = matchingType; }

            public override TestResult Execute(TestExecutionContext context)
            {
                var originalWriter = Console.Out;
                try
                {
                    string consoleOut;
                    using (var writer = new StringWriter())
                    {
                        Console.SetOut(writer);
                        innerCommand.Execute(context);
                        Console.Out.Flush();
                        consoleOut = writer.ToString();
                    }

                    const string OUT = "Console OUT";

                    switch (_matchingType)
                    {
                        case MatchingType.Standard:
                            Assert.That(consoleOut, Is.EqualTo(_expectedMessage), OUT);
                            break;
                        case MatchingType.Regex:
                            Assert.That(consoleOut, Does.Match(_expectedMessage), OUT);
                            break;
                        case MatchingType.StartsWith:
                            Assert.That(consoleOut, Does.StartWith(_expectedMessage), OUT);
                            break;
                        case MatchingType.EndsWith:
                            Assert.That(consoleOut, Does.EndWith(_expectedMessage), OUT);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    context.CurrentResult.SetResult(ResultState.Success);
                }
                catch (AssertionException aex)
                {
                    context.CurrentResult.SetResult(ResultState.Failure, aex.Message);
                }
                catch (Exception ex)
                {
                    context.CurrentResult.SetResult(ResultState.Error, $"Exception occured: {ex}");
                }
                finally
                {
                    Console.SetOut(originalWriter);
                }

                return context.CurrentResult;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleInputAttribute : ConsoleInputOutputBaseAttribute
    {
        public string Message { get; }

        public ConsoleInputAttribute(string message) => Message = message;

        public override TestCommand Wrap(TestCommand command) => new ConsoleInCommand(command, Message);
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleInputSourceAttribute : ConsoleInputOutputBaseAttribute
    {
        public string MemberName { get; }
        public Type DeclaredType { get; }

        public ConsoleInputSourceAttribute(string memberName, Type declaredType = null)
        {
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            DeclaredType = declaredType;
        }

        public override TestCommand Wrap(TestCommand command)
        {
            var type = DeclaredType ?? command.Test.TypeInfo.Type;

            var message = GetText(type, MemberName);

            return new ConsoleInCommand(command, message);
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleOutputAttribute : ConsoleInputOutputBaseAttribute
    {
        public string ExpectedMessage { get; }

        public ConsoleOutputAttribute(string expectedMessage) => ExpectedMessage = expectedMessage;

        public override TestCommand Wrap(TestCommand command) => new ConsoleOutCommand(command, ExpectedMessage, MatchingType.Standard);
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleOutputRegexAttribute : ConsoleInputOutputBaseAttribute
    {
        public string ExpectedPattern { get; }

        public ConsoleOutputRegexAttribute([RegexPattern]string expectedPattern) => ExpectedPattern = expectedPattern;

        public override TestCommand Wrap(TestCommand command) => new ConsoleOutCommand(command, ExpectedPattern, MatchingType.Regex);
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleOutputStartsWithAttribute : ConsoleInputOutputBaseAttribute
    {
        public string StartsWithText { get; }

        public ConsoleOutputStartsWithAttribute(string startsWithText) => StartsWithText = startsWithText;

        public override TestCommand Wrap(TestCommand command) => new ConsoleOutCommand(command, StartsWithText, MatchingType.StartsWith);
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleOutputEndsWithAttribute : ConsoleInputOutputBaseAttribute
    {
        public string EndsWithText { get; }

        public ConsoleOutputEndsWithAttribute(string endsWithText) => EndsWithText = endsWithText;

        public override TestCommand Wrap(TestCommand command) => new ConsoleOutCommand(command, EndsWithText, MatchingType.EndsWith);
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ConsoleOutputSourceAttribute : ConsoleInputOutputBaseAttribute
    {
        public string MemberName { get; }
        public MatchingType MatchingType { get; }
        public Type DeclaredType { get; }

        public ConsoleOutputSourceAttribute(string memberName, MatchingType matchingType = MatchingType.Standard, Type declaredType = null)
        {
            MemberName = memberName;
            MatchingType = matchingType;
            DeclaredType = declaredType;
        }

        public override TestCommand Wrap(TestCommand command)
        {
            var type = DeclaredType ?? command.Test.TypeInfo.Type;

            var expectedMessage = GetText(type, MemberName);

            return new ConsoleOutCommand(command, expectedMessage, MatchingType);
        }
    }
}
