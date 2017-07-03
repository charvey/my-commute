using Data.Feeds;
using System.Linq;
using Xunit;

namespace Tests.Gtfs
{
    public class GtfsFeedTests
    {
        [Fact]
        public void Calendars()
        {
			var feed = SampleFeeds.GoogleSample;

            Assert.Equal(2, feed.Calendars.Count());
        }

        [Theory]
        [InlineData("CITY2", "NADAV", 402, 404)]
        [InlineData("AAMV3", "AMV", 840, 840)]
        public void StopTimes(string tripId, string stopId, int arrival, int departure)
        {
			var feed = SampleFeeds.GoogleSample;

			var stop = feed.StopTimes.Single(st => st.TripId == tripId && st.StopId == stopId);

            Assert.Equal(arrival, stop.ArrivalTime.TotalMinutes);
            Assert.Equal(departure, stop.DepartureTime.TotalMinutes);
        }
    }
}
