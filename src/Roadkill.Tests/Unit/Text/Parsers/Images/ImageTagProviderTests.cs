using NUnit.Framework;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text.Parsers.Images;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text.Parsers.Images
{
	public class ImageTagProviderTests
	{
		private ApplicationSettings _applicationSettings;

		[SetUp]
		public void Setup()
		{
			var container = new MocksAndStubsContainer();
			_applicationSettings = container.ApplicationSettings;
		}

		[Test]
		public void should_convert_to_absolute_path()
		{
			// Arrange
			var resolver = new UrlResolverMock();
			resolver.AbsolutePathSuffix = "123";

			var provider = new ImageTagProvider(_applicationSettings);
			provider.UrlResolver = resolver;

			HtmlImageTag htmlImageTag = new HtmlImageTag("/DSC001.jpg", "/DSC001.jpg", "alt", "title");
			string x = "";

			// Act
			HtmlImageTag actualTag = provider.Parse(htmlImageTag);

			// Assert
			Assert.That(actualTag.Src, Is.EqualTo("/Attachments/DSC001.jpg"));
		}

		[Test]
		public void should_ignore_images_starting_with_http_and_https()
		{
			// Arrange
			var provider = new ImageTagProvider(_applicationSettings);
			
			// Act

			// Assert
		}
	}
}