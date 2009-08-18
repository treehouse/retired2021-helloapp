using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hello.App.Controllers;
using Hello.Utils;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Hello.Tests
{
    public class HomeControllerTests
    {
        public HomeControllerTests()
        {
            var mockSettings = new Mock<ISettingsImpl>();
            mockSettings.Setup(settings => settings.GetString("ConnectionString")).Returns("ConnectionString");
            mockSettings.Setup(s => s.GetString("TwitterBotUsername")).Returns("apphandle");
            Settings.SettingsImplementation = mockSettings.Object;
        }

        [Theory]
        [InlineData("des", "", "", "", "http://twitter.com/?status=%40apphandle+hello+!des")]
        [InlineData("dev", "tag1", "tag2", "", "http://twitter.com/?status=%40apphandle+hello+!dev+%23tag1+%23tag2")]
        public void JoinTest(string userType, string tag1, string tag2, string tag3, string expectedResult)
        {
            var homeController = new HomeController();
            
            var routes = new RouteCollection();
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            
            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            homeController.ControllerContext = new ControllerContext(context.Object, new RouteData(), homeController);
            homeController.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);

            var join = homeController.Join(userType, tag1, tag2, tag3) as RedirectResult;

            Assert.IsType(typeof (RedirectResult), join);

            Assert.Equal(join.Url, expectedResult);
        }

        [Fact]
        public void AboutReturnsActionResult()
        {
            var homeController = new HomeController();

            var About = homeController.About();

            Assert.IsType(typeof (ViewResult), About);
        }

        [Fact]
        public void FaqReturnsActionResult()
        {
            var homeController = new HomeController();

            var faq = homeController.Faq(); 

            Assert.IsType(typeof(ViewResult), faq);
        }
    }
}
