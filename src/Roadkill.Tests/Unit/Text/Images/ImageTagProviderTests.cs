using NUnit.Framework;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text.Parsers.Images;

namespace Roadkill.Tests.Unit.Text.Links
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
		public void should()
		{
			// Arrange
			var provider = new ImageTagProvider(_applicationSettings);

			// Act


			// Assert
		}

		[Test]
		public void imageparsed_should_convert_to_absolute_path()
		{
			//// Arrange
			//UrlResolverMock resolver = new UrlResolverMock();
			//resolver.AbsolutePathSuffix = "123";
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);
			//_markupConverter.UrlResolver = resolver;

			//// Act
			//bool wasCalled = false;
			//_markupConverter.MarkupParser.ImageParsed += (object sender, HtmlImageTag e) =>
			//{
			//	wasCalled = (e.Src == "/Attachments/DSC001.jpg123");
			//};

			//_markupConverter.ToHtml("![Image title](/DSC001.jpg)");

			//// Assert
			//Assert.True(wasCalled, "ImageParsed.ImageEventArgs.Src did not match.");
		}

	}
}