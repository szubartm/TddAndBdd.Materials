using System;

namespace UnitTesting.StaticMethod
{
    #region Original class with call to a static method

    class MyClass
    {
        public string GetValueFromOtherSystem(string request)
        {
            var uniqueId = Guid.NewGuid();
            var response = SlowSystemFacade.ProcessRequest(uniqueId.ToString(), request.ToLower());
            return response.ToUpper();
        }
    }

    #endregion

    #region make sure we can test seam
    class MyClass2
    {
        public string GetValueFromOtherSystem(string request)
        {
            var uniqueId = Guid.NewGuid();
            var response = ProcessRequestSeam(uniqueId.ToString(), request.ToLower());
            return response.ToUpper();
        }

        internal static Func<string, string, string> ProcessRequestSeam = SlowSystemFacade.ProcessRequest;
    }
    #endregion

    #region refactor to dependency and control number of its instances by DI container
    class MyClass3
    {
        private readonly ISlowSystemFacade _slowSystemFacade;
        public MyClass3(ISlowSystemFacade slowSystemFacade) => _slowSystemFacade = slowSystemFacade;

        public string GetValueFromOtherSystem(string request)
        {
            var uniqueId = Guid.NewGuid();
            var response = _slowSystemFacade.ProcessRequest(uniqueId.ToString(), request.ToLower());
            return response.ToUpper();
        }
    }

    interface ISlowSystemFacade
    {
        string ProcessRequest(string id, string request);
    }

    #endregion
}
