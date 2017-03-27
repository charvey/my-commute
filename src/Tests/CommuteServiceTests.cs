using Core;
using Data.Feeds;
using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class CommuteServiceTests
    {
        [Fact]
        public void TestLeave()
        {
            var feed = Feeds.Get("google_bus");
            var now = DateTime.Parse("March 27, 2017 5:00 AM");

            var options = new CommuteService().Get(feed, now);

            Assert.Equal(DateTime.Parse("March 27, 2017 5:22 AM"), options.First().Leave);
        }

        [Fact]
        public void TestArrive()
        {
            var feed = Feeds.Get("google_bus");
            var now = DateTime.Parse("March 27, 2017 5:00 AM");

            var options = new CommuteService().Get(feed, now);

            Assert.Equal(DateTime.Parse("March 27, 2017 6:14 AM"), options.First().Arrive);
        }
    }
}
