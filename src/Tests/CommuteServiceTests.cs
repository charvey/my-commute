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
		[InlineData("July 10, 2017 05:00 AM", "July 10, 2017 05:22 AM", "July 10, 2017 06:09 AM")]
		[InlineData("July 10, 2017 07:55 AM", "July 10, 2017 08:02 AM", "July 10, 2017 08:44 AM")]
		[InlineData("July 10, 2017 12:00 PM", "July 10, 2017 12:17 PM", "July 10, 2017 01:17 PM")]
		[InlineData("July 10, 2017 01:00 PM", "July 10, 2017 02:59 PM", "July 10, 2017 04:22 PM")]
		[InlineData("July 10, 2017 04:30 PM", "July 10, 2017 04:44 PM", "July 10, 2017 05:32 PM")]
		[InlineData("July 10, 2017 06:30 PM", "July 10, 2017 06:39 PM", "July 10, 2017 07:41 PM")]
		public void TypicalScenarios(string now, string leave, string arrive)
        {
			var feed = SeptaFeeds.Latest;

            var options = new CommuteService().Get(feed, DateTime.Parse(now));

            Assert.Equal(DateTime.Parse(leave), options.First().Leave);
			Assert.Equal(DateTime.Parse(arrive), options.First().Arrive);
		}
    }
}
