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
		[InlineData("July 2, 2006", "WE")]
		[InlineData("July 3, 2006", "WE")]
		[InlineData("July 4, 2006", "WE")]
		[InlineData("July 5, 2006", "WD")]
		public void GetApplicableServiceId(string date, string serviceId)
		{
			var feed = SampleFeeds.CalendarSample;

			var actualServiceId = feed.GetApplicableServiceIds(DateTime.Parse(date));

			Assert.Equal(serviceId, actualServiceId.Single());
		}
	}
}
