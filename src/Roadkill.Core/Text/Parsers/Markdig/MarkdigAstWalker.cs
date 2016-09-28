using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Text.Parsers.Markdig
{
    public class MarkdigAstWalker
    {
        private readonly Action<ImageEventArgs> _imageDelegate;
        private readonly Action<LinkEventArgs> _linkDelegate;

        public MarkdigAstWalker(Action<ImageEventArgs> imageDelegate, Action<LinkEventArgs> linkDelegate)
        {
            _imageDelegate = imageDelegate;
            _linkDelegate = linkDelegate;
        }

        public void WalkAndBindParseEvents(MarkdownObject markdownObject)
        {
            foreach (MarkdownObject child in markdownObject.Descendants())
            {
                // LinkInline can be both an <img.. or a <a href="...">
                LinkInline link = child as LinkInline;
                if (link != null)
                {
                    EnsureAttributesInLink(link);

                    if (link.IsImage)
                    {
                        string altText = "";

                        var descendentForAltTag = child.Descendants().FirstOrDefault();
                        if (descendentForAltTag != null)
                            altText = descendentForAltTag.ToString();

                        ImageEventArgs args = InvokeImageParsedEvent(link.Url, altText);

                        // Update the HTML from the data the event gives back
                        link.Url = args.Src;
                        link.Title = altText;

                        // Replace to alt= attribute, it's a literal
                        var literalInline = new LiteralInline(altText);
                        link.FirstChild.ReplaceBy(literalInline);

                        // Necessary for links and Bootstrap 3
                        AddAttribute(link, "border", "0");

                        // Make all images expand via this Bootstrap class 
                        AddClass(link, "img-responsive"); 
                    }
                    else
                    {
                        LinkEventArgs args = InvokeLinkParsedEvent(link.Url, link.Title, link.Label);

                        // Update the HTML from the data the event gives back
                        link.Url = args.Href;
                        AddAttribute(link, "target", args.Target);

                        // TODO: make these configurable (external-links: [])
                        if (!string.IsNullOrEmpty(link.Url) && 
                            (link.Url.ToLower().StartsWith("http://") || 
                            link.Url.ToLower().StartsWith("https://") ||
                            link.Url.ToLower().StartsWith("mailto:"))
                            )
                        {
                            AddAttribute(link, "rel", "nofollow");
                        }

                        if (!string.IsNullOrEmpty(args.CssClass))
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
            // Markdig TODO
            //string linkID = altText.ToLowerInvariant();

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