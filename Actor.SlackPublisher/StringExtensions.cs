namespace SmartCrawler.Actor.SlackPublisher
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool ContainsWord(this string source, string match)
        {
            source = source.Replace("/", " ").Replace("\\", " ").Replace(",", " ");
            return Regex.IsMatch(source, $".*?\\b{match}\\b.*?", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }
    }
}