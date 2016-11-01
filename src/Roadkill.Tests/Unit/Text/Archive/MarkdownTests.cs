using NUnit.Framework;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text.Archive
{
	[TestFixture]
	[Category("Unit")]
	public class MarkdownParserTests
	{
		private PluginFactoryMock _pluginFactory;

		[SetUp]
		public void Setup()
		{
			_pluginFactory = new PluginFactoryMock();
		}

		[Test]
		public void internal_links_with_spaces_should_resolve()
		{
			//// Arrange
			//Page page = new Page() { Id = 1, Title = "My first page" };

			//var settingsRepository = new SettingsRepositoryMock();
			//settingsRepository.SiteSettings = new SiteSettings();

			//PageRepositoryMock pageRepositoryStub = new PageRepositoryMock();
			//pageRepositoryStub.AddNewPage(page, "My first page", "admin", DateTime.UtcNow);

			//ApplicationSettings settings = new ApplicationSettings();
			//settings.Installed = true;

			//UrlResolverMock resolver = new UrlResolverMock();
			//resolver.InternalUrl = "blah";
			//MarkupConverter converter = new MarkupConverter(settings, settingsRepository, pageRepositoryStub, _pluginFactory);
			//converter.UrlResolver = resolver;

			//string markdownText = "[Link](My-first-page)";
			//string invalidMarkdownText = "[Link](My first page)";

			//// Act
			//string expectedHtml = "<p><a href=\"blah\" class=\"missing-page-link\" target=\"\">Link</a></p>\n";
			//string expectedInvalidLinkHtml = "<p>[Link](My first page)</p>\n";

			//string actualHtml = converter.ToHtml(markdownText);
			//string actualHtmlInvalidLink = converter.ToHtml(invalidMarkdownText);

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
			//Assert.That(actualHtmlInvalidLink, Is.EqualTo(expectedInvalidLinkHtml), actualHtmlInvalidLink);
		}

		[Test]
		public void code_blocks_should_allow_quotes()
		{
			//// Issue #82
			//// Arrange
			//Page page = new Page() { Id = 1, Title = "My first page" };

			//PageRepositoryMock pageRepositoryStub = new PageRepositoryMock();
			//pageRepositoryStub.AddNewPage(page, "My first page", "admin", DateTime.UtcNow);

			//var settingsRepository = new SettingsRepositoryMock();
			//settingsRepository.SiteSettings = new SiteSettings();

			//ApplicationSettings settings = new ApplicationSettings();
			//settings.Installed = true;

			//MarkupConverter converter = new MarkupConverter(settings, settingsRepository, pageRepositoryStub, _pluginFactory);

			//string markdownText = "Here is some `// code with a 'quote' in it and another \"quote\"`\n\n" +
			//	"    var x = \"some tabbed code\";\n\n"; // 2 line breaks followed by 4 spaces (tab stop) at the start indicates a code block

			//string expectedHtml = "<p>Here is some <code>// code with a 'quote' in it and another &quot;quote&quot;</code></p>\n" +
			//					"<pre><code>var x = &quot;some tabbed code&quot;;\n" +
			//					"</code></pre>\n";

			//// Act		
			//string actualHtml = converter.ToHtml(markdownText);

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void images_should_support_dimensions()
		{
			//// Arrange
			//Page page = new Page() { Id = 1, Title = "My first page" };

			//PageRepositoryMock pageRepositoryStub = new PageRepositoryMock();
			//pageRepositoryStub.AddNewPage(page, "My first page", "admin", DateTime.UtcNow);

			//var settingsRepository = new SettingsRepositoryMock();
			//settingsRepository.SiteSettings = new SiteSettings();

			//ApplicationSettings settings = new ApplicationSettings();
			//settings.Installed = true;

			//MarkupConverter converter = new MarkupConverter(settings, settingsRepository, pageRepositoryStub, _pluginFactory);

			//string markdownText = "Here is an image:![Image](/Image1.png){style=width:200px;height:200px;} \n\n" +
			//					  "And another with equal dimensions ![Square](/Image1.png){style=width:200px;height:200px;} \n\n" +
			//					  "And this one is a rectangle ![Rectangle](/Image1.png){style='width:250px;height:350px;'}";

			//string expectedHtml = "<p>Here is an image:<img src=\"/Attachments/Image1.png\" class=\"img-responsive\" style=\"width:200px;height:200px;\" border=\"0\" alt=\"Image\" title=\"Image\" /></p>\n" +
			//						"<p>And another with equal dimensions <img src=\"/Attachments/Image1.png\" class=\"img-responsive\" style=\"width:200px;height:200px;\" border=\"0\" alt=\"Square\" title=\"Square\" /></p>\n" +
			//						"<p>And this one is a rectangle <img src=\"/Attachments/Image1.png\" class=\"img-responsive\" style=\"width:250px;height:350px;\" border=\"0\" alt=\"Rectangle\" title=\"Rectangle\" /></p>\n";


			//// Act		
			//string actualHtml = converter.ToHtml(markdownText);

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}
	}
}