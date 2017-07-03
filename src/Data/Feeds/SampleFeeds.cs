using Core.Gtfs;
using Data.Gtfs;

namespace Data.Feeds
{
	public static class SampleFeeds
	{
		public static GtfsFeed CalendarSample => new ZipFileGtfsFeed("Feeds/calendar-sample-feed.zip");
		public static GtfsFeed GoogleSample => new ZipFileGtfsFeed("Feeds/sample-feed.zip");
	}
}
