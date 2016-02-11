namespace SmartCrawler.Actor.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.ServiceFabric.Actors;

    public interface IDouUaActor : IActor
    {
        Task<DateTime> Crawl(DateTime fromDate, string feedUrl);

        Task<DateTime> GetLastRun();
    }
}