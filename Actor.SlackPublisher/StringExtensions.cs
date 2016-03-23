namespace SmartCrawler.Actor.SlackPublisher
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool ContainsWord(this string source, string match)
        {
            match = match.Replace("#", "sharp");
            source = source.Replace("/", " ").Replace("\\", " ").Replace(",", " ").Replace("#", "sharp");
            return Regex.IsMatch(source, $".*?\\b{match}\\b.*?", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }
    }
}