namespace SmartCrawler.Actor.SlackPublisher
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string SlackHookUrl { get; } = ConfigurationManager.AppSettings["SlackHookUrl"];

        public static string SlackHookName { get; } = ConfigurationManager.AppSettings["SlackHookName"];

        public static string SlackHookEmoji { get; } = ConfigurationManager.AppSettings["SlackHookEmoji"];
    }
}