using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueueAPI.Controllers;

namespace RESTQueue.UnitTests.APITests
{
    [TestClass]
    public class QueryControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNull()
        {
            var qController = new QueryController(null, null, null, null, null);
        }
    }
}