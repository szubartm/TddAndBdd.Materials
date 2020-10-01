using System.Collections.Generic;
using System.Linq;

namespace Common.TestUtils
{
    /// <summary>
    /// Various extensions used for testing purposes
    /// </summary>
    public static class TestDataExtensions
    {
        public static T Second<T>(this IEnumerable<T> collection) => collection.Take(2).Last();
        
        public static T Third<T>(this IEnumerable<T> collection) => collection.Take(3).Last();
    }
}
