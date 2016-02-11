namespace SmartCrawler.Actor.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.ServiceFabric.Actors;

    public interface ISlackPublisher : IActor
    {
        Task<bool> Publish(IEnumerable<Item> items);
    }
}