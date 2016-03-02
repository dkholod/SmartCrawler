namespace SmartCrawler.Actor.SlackPublisher
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ServiceFabric.Actors;

    using SmartCrawler.Actor.Interfaces;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class SlackPublisher : StatelessActor, ISlackPublisher
    {
        public Task<bool> Publish(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                var matchInfo = IsMatched(item);
                if (matchInfo.IsMatched)
                {
                    const string Template = "<{0}|{1}> \n&gt;{2} ...\n";
                    var text = item.Text.Trim().Substring(0, 200).Replace("\n\n", "\n").Replace("\n", "\n>");
                    Slack.Post(string.Format(Template, item.Link, item.Title, text), matchInfo.KeyWords);

                    // debugging
                    // WriteToFile(item);
                }
            }

            return Task.FromResult(true);
        }

        private void WriteToFile(Item item)
        {
            using (var sw = File.CreateText(path: "C:\\temp\\" + GetFileName(item.PubDate)))
            {
                sw.WriteLine(item.Title);
                sw.WriteLine(item.PubDate);
                sw.WriteLine(item.Link);
                sw.WriteLine(item.Text);
            }
        }

        private static MatchInfo IsMatched(Item item)
        {
            var filters = ConfigurationManager.AppSettings[item.FromFeed];
            if (string.IsNullOrWhiteSpace(filters))
            {
                // always match if no filters are specified
                return MatchInfo.SuccessfulEmpty;
            }

            var keywords = NormalizeFilters(filters);
            return new MatchInfo(keywords
                .Where(keyword => item.Title.ContainsWord(keyword) || item.Text.ContainsWord(keyword)));
        }

        private static IEnumerable<string> NormalizeFilters(string filters)
        {
            return filters.Split(',')
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrWhiteSpace(f));

        }

        private string GetFileName(DateTime pubDate)
        {
            var actorId = this.Id.GetStringId();
            return $"{pubDate.Year}_{pubDate.Month}_{pubDate.Day}_{pubDate.Hour}_{pubDate.Minute}_{pubDate.Second}_{actorId}_{Guid.NewGuid().ToString().Substring(0, 4)}.txt";
        }
    }
}
