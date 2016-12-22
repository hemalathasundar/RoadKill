namespace Roadkill.Core.Text.Parsers.Links
{
	public interface IHtmlLinkTagConverter
	{
		bool IsMatch(HtmlLinkTag htmlLinkTag);
		HtmlLinkTag Convert(HtmlLinkTag htmlLinkTag);
	}
}