using System;
using System.Collections.Generic;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Text.Parsers.Markdig
{
    public class MarkdigWalker
    {
        private readonly Action<ImageEventArgs> _imageDelegate;
        private readonly Action<LinkEventArgs> _linkDelegate;

        public MarkdigWalker(Action<ImageEventArgs> imageDelegate, Action<LinkEventArgs> linkDelegate)
        {
            _imageDelegate = imageDelegate;
            _linkDelegate = linkDelegate;
        }

        public void WalkAndBindParseEvents(MarkdownObject markdownObject)
        {
            foreach (MarkdownObject child in markdownObject.Descendants())
            {
                // LinkInline can be both an image or a <a href="...">
                LinkInline link = child as LinkInline;
                if (link != null)
                {
                    EnsureAttributesInLink(link);

                    if (link.IsImage)
                    {
                        ImageEventArgs args = InvokeImageParsedEvent(link.Url, link.Title);

                        // Update the HTML from the data the event gives back
                        link.Url = args.Src;
                        AddAttribute(link, "alt", args.Alt);
                    }
                    else
                    {
                        LinkEventArgs args = InvokeLinkParsedEvent(link.Url, link.Title, link.Label);

                        // Update the HTML from the data the event gives back
                        link.Url = args.Href;
                        AddAttribute(link, "target", args.Target);
                        AddClass(link, args.CssClass);
                    }
                }

                WalkAndBindParseEvents(child);
            }
        }

        private void EnsureAttributesInLink(LinkInline link)
        {
            HtmlAttributes attributes = link.GetAttributes();
            if (attributes == null)
            {
                attributes = new HtmlAttributes();
                attributes.Classes = new List<string>();
            }

            if (attributes.Classes == null)
            {
                attributes.Classes = new List<string>();
            }
        }

        private void AddAttribute(LinkInline link, string name, string value)
        {
            HtmlAttributes attributes = link.GetAttributes();
            attributes.AddPropertyIfNotExist(name, value);

            link.SetAttributes(attributes);
        }

        private void AddClass(LinkInline link, string cssClass)
        {
            HtmlAttributes attributes = link.GetAttributes();
            attributes.Classes.Add(cssClass);
            link.SetAttributes(attributes);
        }

        private ImageEventArgs InvokeImageParsedEvent(string url, string altText)
        {
            string linkID = altText.ToLowerInvariant();
            ImageEventArgs args = new ImageEventArgs(url, url, altText, "");
            _imageDelegate(args);

            return args;
        }

        private LinkEventArgs InvokeLinkParsedEvent(string url, string text, string target)
        {
            LinkEventArgs args = new LinkEventArgs(url, url, text, target);
            _linkDelegate(args);

            return args;
        }
    }
}