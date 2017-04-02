using Core.Gtfs;
using Data.Gtfs;
using System.Linq;

namespace Data.Feeds
{
    public static class Feeds
    {
        public static GtfsFeed Get(string feedId)
        {
            return new ZipFileGtfsFeed($"Feeds/{feedId}.zip");
        }

        public static GtfsFeed Get(params string[] feedIds)
        {
            return new AggregateGtfsFeed(feedIds.Select(Get));
        }
    }
}
