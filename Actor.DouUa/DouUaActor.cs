namespace SmartCrawler.Actor.DouUa
{
    using System;
    using System.Fabric;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ServiceFabric.Actors;

    using SmartCrawler.Actor.Interfaces;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DouUaActor : StatefulActor<ReccurentActorState>, IDouUaActor, IRemindable
    {
        private const string CrawlReminder = "CrawlReminder@DouUaActor";

        public Task<DateTime> Crawl(DateTime fromDate, string feedUrl)
        {
            var data =  WebCrawlers.DouUa.getCrawlerData(fromDate, feedUrl);
            var items = data.Items.Select(i => new Item
            {
                PubDate = i.PubDate,
                Link = i.Link,
                Title = i.Title ?? string.Empty,
                Text = i.Description ?? string.Empty,
                FromFeed = feedUrl
            });

            var actor = ActorProxy.Create<ISlackPublisher>(new ActorId("PublishActor_for_" + this.Id.GetStringId()), "fabric:/SmartCrawler");
            actor.Publish(items);

            return Task.FromResult(data.RecentDate);
        }

        public Task<DateTime> GetLastRun()
        {
            return Task.FromResult(this.State.LastRunOn);
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals(CrawlReminder))
            {
                var feedName = AppSettings.GetRssFeed(this.Id.GetStringId());
                if (!string.IsNullOrWhiteSpace(feedName))
                {
                    this.State.LastItemDate = this.Crawl(this.State.LastItemDate, feedName).Result;
                }
            }

            this.State.LastRunOn = DateTime.UtcNow;
            return Task.FromResult(true);
        }

        protected override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                // This is the first time this actor has ever been activated.
                // Set the actor's initial start date to crawl is Now-10 days.
                this.State = new ReccurentActorState
                {
                    LastItemDate = DateTime.UtcNow.AddDays(-10),
                    LastRunOn = DateTime.UtcNow
                };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            
            if (!ReminderIsRegistered(CrawlReminder))
            {
                // Register reminder if not yet registered            
                var reminderRegistration = this.RegisterReminderAsync(
                    CrawlReminder,
                    null,
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromMinutes(AppSettings.RecurrencyIntervalInMinutes),
                    ActorReminderAttributes.None).Result;

                ActorEventSource.Current.ActorMessage(this, "Registering reminder {0}", reminderRegistration.Name);
            }

            return Task.FromResult(true);
        }

        private bool ReminderIsRegistered(string name)
        {
            try
            {
                var reminder = this.GetReminder(name);
                return reminder != null;
            }
            catch (FabricException)
            {
                return false;
            }
        }
    }
}