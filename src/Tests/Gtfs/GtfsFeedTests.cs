using Data.Feeds;
using System.Linq;
using Xunit;

namespace Tests.Gtfs
{
    public class GtfsFeedTests
    {
        [Theory]
        [InlineData("sample-feed", 2)]
        public void Calendars(string feedId, int count)
        {
            var feed = Feeds.Get(feedId);

            Assert.Equal(count, feed.Calendars.Count());
        }

        [Theory]
        [InlineData("sample-feed", "CITY2", "NADAV", 402, 404)]
        [InlineData("sample-feed", "AAMV3", "AMV", 840, 840)]
        public void StopTimes(string feedId, string tripId, string stopId, int arrival, int departure)
        {
            var feed = Feeds.Get(feedId);

            var stop = feed.StopTimes.Single(st => st.TripId == tripId && st.StopId == stopId);

            Assert.Equal(arrival, stop.ArrivalTime.TotalMinutes);
            Assert.Equal(departure, stop.DepartureTime.TotalMinutes);
        }
    }
}
