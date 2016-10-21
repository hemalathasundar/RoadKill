using System.IO;
using System.Text;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using NUnit.Framework;
using Roadkill.Core.Text.Parsers.Markdig;

namespace Roadkill.Tests.Unit.Text
{
    public class MarkdigImagesAndLinkWalkerTests
    {
        private MarkdownDocument CreateMarkdownObject(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            MarkdownDocument doc = Markdown.Parse(markdown, pipeline);
            return doc;
        }

        private string ConvertToHtml(MarkdownObject markdownObject)
        {
            var builder = new StringBuilder();
            var textwriter = new StringWriter(builder);

            var renderer = new HtmlRenderer(textwriter);
            renderer.Render(markdownObject);

            return builder.ToString();
        }

        [Test]
        public void should_parse_basic_image_and_link_markdown()
        {
            // given
            bool imageParsed = false;
            bool linkParsed = false;

            MarkdownDocument markdownObject = CreateMarkdownObject("[text](serverless.html)");
            var walker = new MarkdigImagesAndLinkWalker(image => { imageParsed = true; }, link => { linkParsed = true; });

            // when
            walker.WalkAndBindParseEvents(markdownObject);

            // walk
            Assert.True(imageParsed);
            Assert.True(linkParsed);
        }

        [Test]
        public void should_ignore_null_image_handler()
        {
            // given
            MarkdownDocument markdownObject = CreateMarkdownObject("![img](serverless.jpg)");
            var walker = new MarkdigImagesAndLinkWalker(null, link => { });

            // when + then
            walker.WalkAndBindParseEvents(markdownObject);
        }

        [Test]
        public void should_ignore_null_link_handler()
        {
            // given
            MarkdownDocument markdownObject = CreateMarkdownObject("[text](serverless.hyml)");
            var walker = new MarkdigImagesAndLinkWalker(image => { }, null);

            // when + then
            walker.WalkAndBindParseEvents(markdownObject);
        }

        [Test]
        public void should_add_css_and_attributes_to_links_from_delegate()
        {
            // given
            MarkdownDocument markdownObject = CreateMarkdownObject("[text](serverless.html)");
            var walker = new MarkdigImagesAndLinkWalker(null, link =>
            {
                link.CssClass = "my-class";
                link.Href = "new-href";
                link.Target = "new-target";
                link.Text = "new-text";
                link.IsInternalLink = true;
            });

            // when
            walker.WalkAndBindParseEvents(markdownObject);
            string html = ConvertToHtml(markdownObject);

            // then
            Assert.That(html, Is.EqualTo("<p><a href=\"new-href\" class=\"my-class\" target=\"new-target\">new-text</a></p>\n"));
        }

        [Test]
        public void should_parse_link_protocols()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }

        [Test]
        public void should_parse_images()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }

        [Test]
        public void should_add_css_and_attributes_to_images()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }

        [Test]
        public void should_handle_empty_strings()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }
    }
}