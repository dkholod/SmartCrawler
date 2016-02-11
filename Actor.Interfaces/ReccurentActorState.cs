namespace SmartCrawler.Actor.Interfaces
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    public class ReccurentActorState
    {
        [DataMember]
        public DateTime LastRunOn { get; set; }

        [DataMember]
        public DateTime LastItemDate { get; set; }

        public override string ToString()
        {
            return string
                .Format(CultureInfo.InvariantCulture, "ReccurentActorState[LastRunOn = {0}, LastItemDate = {1}]", LastRunOn, LastItemDate);
        }
    }
}