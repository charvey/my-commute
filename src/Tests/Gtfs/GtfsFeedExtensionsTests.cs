using Core.Gtfs;
using Data.Feeds;
using System;
using System.Linq;
using Xunit;

namespace Tests.Gtfs
{
    public class GtfsFeedExtensionsTests
    {
        [Theory]
        [InlineData("calendar-sample-feed", "July 2, 2006", "WE")]
        [InlineData("calendar-sample-feed", "July 3, 2006", "WE")]
        [InlineData("calendar-sample-feed", "July 4, 2006", "WE")]
        [InlineData("calendar-sample-feed", "July 5, 2006", "WD")]
        public void GetApplicableServiceId(string feedId, string date, string serviceId)
        {
            var feed = Feeds.Get(feedId);

            var actualServiceId = feed.GetApplicableServiceIds(DateTime.Parse(date));

            Assert.Equal(serviceId, actualServiceId.Single());
        }
    }
}
