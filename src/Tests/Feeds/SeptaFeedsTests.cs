using Core.Gtfs;
using Data.Feeds;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Feeds
{
    public class SeptaFeedsTests
    {
		[Fact]
		public void SeptaLatestIsRelevant()
		{
			var feed = SeptaFeeds.Latest;

			var relevantServiceIds = feed.GetApplicableServiceIds(DateTime.Now);

			Assert.NotEmpty(relevantServiceIds);
		}
    }
}
