using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Common.TestUtils
{
    /// <summary>
    /// A bunch of useful extensions for assertions, hiding notorious problem with assertion attributes: 
    /// Example: Assert.IsEqual(actual,expected) or the other way round: Assert.IsEqual(expected, actual)?
    /// </summary>
    public static class NUnitExtensions
    {
        public static void ShouldBeNull<T>(this T objectUnderTest) 
            => Assert.IsNull(objectUnderTest);

        public static void ShouldNotBeNull<T>(this T objectUnderTest) 
            => Assert.IsNotNull(objectUnderTest);

        public static void ShouldBeTrue(this bool result) 
            => Assert.IsTrue(result);

        public static void ShouldBeFalse(this bool result) 
            => Assert.IsFalse(result);

        public static void ShouldBeEqualTo<T>(this T objectUnderTest, T expected) 
            => Assert.AreEqual(expected, objectUnderTest);

        public static void ShouldNotBeEqualTo<T>(this T objectUnderTest, T expected) 
            => Assert.AreNotEqual(expected, objectUnderTest);

        public static void ShouldBeTheSameAs<T>(this T objectUnderTest, T expected) 
            => Assert.AreSame(expected, objectUnderTest);

        public static void ShouldNotBeTheSameAs<T>(this T objectUnderTest, T expected) 
            => Assert.AreNotSame(expected, objectUnderTest);

        public static void ShouldBeAnInstanceOf<T>(this object objectUnderTest) 
            => Assert.IsInstanceOf<T>(objectUnderTest);

        public static void ShouldContain(this string objectUnderTest, string text) 
            => objectUnderTest.Contains(text).ShouldBeTrue();

        public static void ShouldNotContain(this string objectUnderTest, string text) 
            => objectUnderTest.Contains(text).ShouldBeFalse();

        public static void ShouldContain<T>(this IEnumerable<T> objectUnderTest, T item) 
            => objectUnderTest.Any(x => x.Equals(item)).ShouldBeTrue();

        public static void ShouldNotContain<T>(this IEnumerable<T> objectUnderTest, T item) 
            => objectUnderTest.Any(x => x.Equals(item)).ShouldBeFalse();

        public static void ShouldBeEmpty<T>(this IEnumerable<T> objectUnderTest) 
            => objectUnderTest.Any().ShouldBeFalse();

        public static void ShouldNotBeEmpty<T>(this IEnumerable<T> objectUnderTest) 
            => objectUnderTest.Any().ShouldBeTrue();

        public static void ShouldBeLessThan(this IComparable objectUnderTest, IComparable item) 
            => Assert.Less(objectUnderTest, item);

        public static void ShouldBeLessOrEqualThan(this IComparable objectUnderTest, IComparable item) 
            => Assert.LessOrEqual(objectUnderTest, item);

        public static void ShouldBeGreaterThan(this IComparable objectUnderTest, IComparable item) 
            => Assert.Greater(objectUnderTest, item);

        public static void ShouldBeGreaterOrEqualThan(this IComparable objectUnderTest, IComparable item) 
            => Assert.GreaterOrEqual(objectUnderTest, item);
    }
}
