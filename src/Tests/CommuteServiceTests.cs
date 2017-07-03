using Core;
using Data.Feeds;
using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class CommuteServiceTests
    {
        [Theory]
        [InlineData("July 27, 2017 5:00 AM", "July 27, 2017 5:22 AM")]
        [InlineData("July 27, 2017 8:45 PM", "July 27, 2017 8:57 PM")]
        public void TestLeave(string now, string leave)
        {
			var feed = SeptaFeeds.Latest;

            var options = new CommuteService().Get(feed, DateTime.Parse(now));

            Assert.Equal(DateTime.Parse(leave), options.First().Leave);
        }

        [Theory]
        [InlineData("July 27, 2017 5:00 AM", "July 27, 2017 6:09 AM")]
        [InlineData("July 27, 2017 8:45 PM", "July 27, 2017 9:58 PM")]
        public void TestArrive(string now, string arrive)
        {
			var feed = SeptaFeeds.Latest;

            var options = new CommuteService().Get(feed, DateTime.Parse(now));

            Assert.Equal(DateTime.Parse(arrive), options.First().Arrive);
        }
    }
}
