using System.Text.RegularExpressions;

namespace NMS.Assistant.Integration.Mapper
{
    public static class RuntimeMapper
    {
        public static int ToSeconds(string runtimeString)
        {
            if (string.IsNullOrEmpty(runtimeString)) return 0;

            Regex runtimeSecondsRegex = new Regex(@"(\d{2})\s*\w*\s*(\d{2})\s*\w*");
            if (!runtimeSecondsRegex.IsMatch(runtimeString)) return 0;

            Match match = runtimeSecondsRegex.Match(runtimeString);
            if (match.Groups.Count != 3) return 0;

            string minString = match.Groups[1].Value;
            if (!int.TryParse(minString, out int min)) return 0;

            string secString = match.Groups[2].Value;
            if (!int.TryParse(secString, out int sec)) return 0;

            return (min * 60) + sec;
        }
    }
}
