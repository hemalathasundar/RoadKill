using System.Web.Mvc;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class UrlHelperMock : UrlHelper
	{
		public string ExpectedContent { get; set; }
		public string ExpectedAction { get; set; }

		public override string Content(string path)
		{
			return path;
		}

		public override string Action(string actionName, string controllerName, object routeValues)
		{
			return ExpectedAction;
		}
	}
}