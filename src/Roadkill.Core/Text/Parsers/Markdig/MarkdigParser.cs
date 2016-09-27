using System;
using System.IO;
using System.Text;
using Markdig;
using Markdig.Renderers;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Text.Parsers.Markdig
{
    public class MarkdigParser : IMarkupParser
    {
        public event EventHandler<ImageEventArgs> ImageParsed;
        public event EventHandler<LinkEventArgs> LinkParsed;

        public string ToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
                return "";

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var doc = Markdown.Parse(markdown, pipeline);
            var walker = new MarkdigWalker(OnImageParsed, OnLinkParsed);

            walker.WalkAndBindParseEvents(doc);

            var builder = new StringBuilder();
            var textwriter = new StringWriter(builder);

            var renderer = new HtmlRenderer(textwriter);
            renderer.Render(doc);

            return builder.ToString();
        }

        /// <summary>
        /// Raises the <see cref="ImageParsed"/> event.
        /// </summary>
        /// <param name="e">The event data. </param>
        protected void OnImageParsed(ImageEventArgs e)
        {
            if (ImageParsed != null)
                ImageParsed(this, e);
        }

        /// <summary>
        /// Raises the <see cref="LinkParsed"/> event.
        /// </summary>
        /// <param name="e">The event data. </param>
        protected void OnLinkParsed(LinkEventArgs e)
        {
            if (LinkParsed != null)
                LinkParsed(this, e);
        }
    }
}
