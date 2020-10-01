using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace UnitTesting.StaticMethod
{
    [TestFixture]
    public class MyClassTest
    {
        [Test]
        public void ShouldGetAppropriateResponse()
        {
            var objectUnderTest = new MyClass();

            var response = objectUnderTest.GetValueFromOtherSystem("dupa");

            Assert.That(response, Is.EqualTo("DUPA"));
        }
    }

    [TestFixture]
    public class MyClass2Test
    {
        Func<string, string, string> _originalBehaviour;

        [Test]
        public void ShouldGetAppropriateResponse()
        {
            var objectUnderTest = new MyClass2();
            var originalSeam = MyClass2.ProcessRequestSeam;

            var ids = new List<string>();
            var requests = new List<string>();

            _originalBehaviour = MyClass2.ProcessRequestSeam;
            MyClass2.ProcessRequestSeam = (id, request) =>
            {
                ids.Add(id);
                requests.Add(request);
                return request + "123";
            };

            var response1 = objectUnderTest.GetValueFromOtherSystem("dupa");
            var response2 = objectUnderTest.GetValueFromOtherSystem("dupa");

             MyClass2.ProcessRequestSeam = _originalBehaviour;

            Assert.That(response1, Is.EqualTo("DUPA123"));
            Assert.That(response2, Is.EqualTo("DUPA123"));

            Assert.That(ids.Count,Is.EqualTo(2));
            Assert.That(ids.Distinct().Count() == 2);
            Assert.That(requests.Distinct().Count() == 1);
        }
    }
}