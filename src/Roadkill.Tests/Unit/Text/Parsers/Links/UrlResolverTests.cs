using NUnit.Framework;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Tests.Unit.Text.Parsers.Links
{
	public class UrlResolverTests
	{
		[SetUp]
		public void Setup()
		{
			var container = new MocksAndStubsContainer();
		}

		[Test]
		public void should()
		{
			// Arrange
			var resolver = new UrlResolver();

			// Act

			// Assert
			Assert.Fail("todo");
		}
	}
}