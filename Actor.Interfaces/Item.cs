namespace SmartCrawler.Actor.Interfaces
{
    using System;

    public class Item
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public DateTime PubDate { get; set; }

        public string Text { get; set; }

        public string FromFeed { get; set; }
    }
}