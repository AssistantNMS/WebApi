using System.Text.RegularExpressions;

namespace NMS.Assistant.Domain.Helper
{
    public static class HtmlHelper
    {
        public static string CleanHtml(this string html)
        {
            string htmlDocument = html
                .Replace("<br><br>", "\n")
                .Replace("<br>", "\n")
                .Replace("</li>", "\r\n");
            string noTags = Regex.Replace(htmlDocument, @"<[^>]*>", string.Empty).Trim();
            return noTags.AddSpaceAfterSpecialCharacter('!')
                .AddSpaceAfterSpecialCharacter('.')
                .AddSpaceAfterSpecialCharacter(',')
                .AddSpaceBeforeCapitals();
        }

        public static string AddSpaceAfterSpecialCharacter(this string html, char specialChar)
        {
            string pattern = $@"\{specialChar}[^\s-]";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(html);
            foreach (Match match in matches)
            {
                string charToKeep = match.Value.Replace(specialChar.ToString(), string.Empty);
                html = html.Replace(match.Value, charToKeep);
            }
            return html;
        }

        public static string AddSpaceBeforeCapitals(this string html)
        {
            string pattern = @"[^\s-][A-Z]";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(html);
            foreach (Match match in matches)
            {
                string newValue = match.Value[0] + ". " + match.Value[1];
                html = html.Replace(match.Value, newValue);
            }
            return html;
        }
    }
}
