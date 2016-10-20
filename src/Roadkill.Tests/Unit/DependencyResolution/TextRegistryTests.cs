using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.Converters;
using Roadkill.Core.DependencyResolution.StructureMap;
using Roadkill.Core.Plugins;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;
using StructureMap;

namespace Roadkill.Tests.Unit.DependencyResolution
{
	[TestFixture]
	[Category("Unit")]
	public class TextRegistryTests
    {
		private IContainer CreateContainer()
		{
			var registry = new TextRegistry();
			var container = new Container(c =>
			{
				c.AddRegistry(registry);
			});

			return container;
		}

		private void AssertDefaultType<TParent, TConcrete>(IContainer container = null)
		{
			// Arrange
			if (container == null)
				container = CreateContainer();

			// Act
			TParent instance = container.GetInstance<TParent>();

			// Assert
			Assert.That(instance, Is.TypeOf<TConcrete>());
		}

		[Test]
		public void should_construct_builder_and_parse_basic_markup()
		{
			// Arrange
			IContainer container = CreateContainer();

			// Act
		    var builder = container.GetInstance<TextMiddlewareBuilder>();

            // Assert
            Assert.That(builder, Is.Not.Null);

		    string html = builder.Execute("**markdown**");
            Assert.That(html, Is.EqualTo("<p><strong>markdown</strong></p>\n")); // a basic smoke test of the middleware chain
        }

        [Test]
		public void should_register_types_with_instances()
		{
			// Arrange + Act + Assert
			AssertDefaultType<IPluginFactory, PluginFactory>();
            AssertDefaultType<IMarkupParser, MarkdigParser>();
            AssertDefaultType<IHtmlSanitizerFactory, HtmlSanitizerFactory>();
            AssertDefaultType<CustomTokenParser, CustomTokenParser>();
        }

        [Test]
        public void should_register_middleware_in_the_correct_order()
        {
            // Arrange
            IContainer container = CreateContainer();

            // Act
            var builder = container.GetInstance<TextMiddlewareBuilder>();

            // Assert
            Assert.That(builder, Is.Not.Null);
            Assert.That(builder.MiddleItems[0], Is.TypeOf<TextPluginBeforeParseMiddleware>());
            Assert.That(builder.MiddleItems[1], Is.TypeOf<MarkupParserMiddleware>());
            Assert.That(builder.MiddleItems[2], Is.TypeOf<HarmfulTagMiddleware>());
            Assert.That(builder.MiddleItems[3], Is.TypeOf<CustomTokenMiddleware>());
            Assert.That(builder.MiddleItems[4], Is.TypeOf<TextPluginAfterParseMiddleware>());
        }
    }
}
