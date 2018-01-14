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
		[InlineData("January 22, 2018 05:00 AM", "January 22, 2018 05:22 AM", "January 22, 2018 06:09 AM")]
		[InlineData("January 22, 2018 07:55 AM", "January 22, 2018 08:02 AM", "January 22, 2018 09:09 AM")]
		[InlineData("January 22, 2018 12:00 PM", "January 22, 2018 12:17 PM", "January 22, 2018 01:17 PM")]
		[InlineData("January 22, 2018 01:00 PM", "January 22, 2018 02:59 PM", "January 22, 2018 04:22 PM")]
		[InlineData("January 22, 2018 04:30 PM", "January 22, 2018 04:44 PM", "January 22, 2018 05:32 PM")]
		[InlineData("January 22, 2018 06:30 PM", "January 22, 2018 06:39 PM", "January 22, 2018 07:41 PM")]
		public void TypicalScenarios(string now, string leave, string arrive)
        {
			var feed = SeptaFeeds.Latest;

            var options = new CommuteService().Get(feed, DateTime.Parse(now));

            Assert.Equal(DateTime.Parse(leave), options.First().Leave);
			Assert.Equal(DateTime.Parse(arrive), options.First().Arrive);
		}
    }
}
