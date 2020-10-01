using System.Threading;

namespace UnitTesting.StaticMethod
{
    class SlowSystemFacade
    {
        public static string ProcessRequest(string id, string request)
        {
            Thread.Sleep(10*1000);
            return "blah-blah-blah";
        }
    }
}