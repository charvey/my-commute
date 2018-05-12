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
		[InlineData("05:00 AM", "05:22 AM", "06:09 AM")]
		[InlineData("07:55 AM", "08:02 AM", "09:09 AM")]
		[InlineData("12:00 PM", "12:17 PM", "01:17 PM")]
		[InlineData("01:00 PM", "02:59 PM", "04:22 PM")]
		[InlineData("03:45 PM", "03:59 PM", "04:59 PM")]
		[InlineData("04:30 PM", "04:44 PM", "06:07 PM")]
		[InlineData("06:30 PM", "06:39 PM", "07:41 PM")]
		public void TypicalScenarios(string now, string leave, string arrive)
		{
			var day = Next(DayOfWeek.Monday);
			var feed = SeptaFeeds.Latest;

			var options = new CommuteService().Get(feed, DateTime.Parse($"{day:D} {now}"));

			Assert.Equal(DateTime.Parse($"{day:D} {leave}"), options.First().Leave);
			Assert.Equal(DateTime.Parse($"{day:D} {arrive}"), options.First().Arrive);
		}

		private DateTime Next(DayOfWeek dayOfWeek)
		{
			var current = DateTime.Today;
			while (current.DayOfWeek != dayOfWeek)
				current = current.AddDays(1);
			return current;
		}
    }
}
