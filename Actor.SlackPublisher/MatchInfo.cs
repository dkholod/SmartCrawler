namespace SmartCrawler.Actor.SlackPublisher
{
    using System.Collections.Generic;
    using System.Linq;

    public class MatchInfo
    {
        public static MatchInfo SuccessfulEmpty = new MatchInfo { IsMatched = true, KeyWords = Enumerable.Empty<string>() };

        private MatchInfo()
        { }

        public MatchInfo(IEnumerable<string> keyWords)
            : this()
        {
            IsMatched = keyWords.Any();
            KeyWords = keyWords;
        }

        public bool IsMatched { get; private set; }

        public IEnumerable<string> KeyWords { get; private set; }
    }
}