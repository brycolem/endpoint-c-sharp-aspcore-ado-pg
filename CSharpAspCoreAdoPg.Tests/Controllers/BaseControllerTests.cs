using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CSharpAspCoreAdoPg.Controllers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CSharpAspCoreAdoPg.Models;

namespace CSharpAspCoreAdoPg.Tests.Controllers
{
  public class BaseControllerTests
  {
    private readonly BaseController _controller;

    public BaseControllerTests()
    {
      _controller = new BaseController();
    }

    [Fact]
    public void GetApiOverview_ReturnsOkResult_WithExpectedLinks()
    {
      var mockRequest = new Mock<HttpRequest>();
      mockRequest.SetupGet(r => r.Scheme).Returns("http");
      mockRequest.SetupGet(r => r.Host).Returns(new HostString("localhost", 5000));
      mockRequest.SetupGet(r => r.PathBase).Returns("/");

      var mockHttpContext = new Mock<HttpContext>();
      mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = mockHttpContext.Object
      };

      var result = _controller.GetApiOverview();
      var okResult = Assert.IsType<OkObjectResult>(result);
      var responseValue = Assert.IsType<ApiOverview>(okResult.Value);

      var links = responseValue._links;
      Assert.Equal(8, links.Count);
      Assert.Contains("self", links.Keys);
      Assert.Contains("swagger_docs", links.Keys);
      Assert.Contains("get_all_applications", links.Keys);
      Assert.Contains("get_application", links.Keys);
      Assert.Contains("create_application", links.Keys);
      Assert.Contains("update_application", links.Keys);
      Assert.Contains("patch_application", links.Keys);
      Assert.Contains("delete_application", links.Keys);

      var selfLink = links["self"];
      Assert.Equal("http://localhost:5000/", selfLink["href"]);
      Assert.Equal("GET", selfLink["method"]);
    }

    [Fact]
    public void GetApiOverview_ReturnsOkResult_WithExpectedLinks_WhenBaseUrlDoesNotEndWithSlash()
    {
      var mockRequest = new Mock<HttpRequest>();
      mockRequest.SetupGet(r => r.Scheme).Returns("http");
      mockRequest.SetupGet(r => r.Host).Returns(new HostString("localhost", 5000));
      mockRequest.SetupGet(r => r.PathBase).Returns("");

      var mockHttpContext = new Mock<HttpContext>();
      mockHttpContext.SetupGet(c => c.Request).Returns(mockRequest.Object);

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = mockHttpContext.Object
      };

      var result = _controller.GetApiOverview();
      var okResult = Assert.IsType<OkObjectResult>(result);
      var responseValue = Assert.IsType<ApiOverview>(okResult.Value);

      var links = responseValue._links;
      Assert.Equal(8, links.Count);
      Assert.Contains("self", links.Keys);

      var selfLink = links["self"];
      Assert.Equal("http://localhost:5000/", selfLink["href"]);
      Assert.Equal("GET", selfLink["method"]);
    }
  }
}
