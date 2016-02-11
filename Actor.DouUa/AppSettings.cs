namespace SmartCrawler.Actor.DouUa
{
    using System.Configuration;

    public static class AppSettings
    {
        public static int RecurrencyIntervalInMinutes
        {
            get
            {
                int intervalInMinutes;
                if (!int.TryParse(ConfigurationManager.AppSettings["RecurrencyIntervalInMinutes"], out intervalInMinutes))
                {
                    intervalInMinutes = 2;
                }

                return intervalInMinutes;
            }
        }

        public static string GetRssFeed(string feedName) => ConfigurationManager.AppSettings[feedName];
    }
}