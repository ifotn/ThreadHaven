using Microsoft.AspNetCore.Mvc;
using ThreadHaven.Controllers;

namespace ThreadHavenTesting
{
    [TestClass]
    public class HomeControllerTests
    {
        // test name describes method being tested + expected result
        [TestMethod]
        public void IndexReturnsView()
        {
            // 3 Steps to a Unit Test:
            // 1. Arrange: set up any vars / objects needed
            HomeController controller = new HomeController();

            // 2. Act: execute the method + condition we want to test, get a result back
            var result = (ViewResult)controller.Index();

            // 3. Assert: evalute result - did we get what we expected?
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void PrivacyReturnsView()
        {
            // arrange
            HomeController controller = new HomeController();

            // act & convert IActionResult => ViewResult
            var result = (ViewResult)controller.Privacy();

            // assert
            Assert.AreEqual("Privacy", result.ViewName);
        }
    }
}