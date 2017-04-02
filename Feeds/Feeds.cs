using Core.Gtfs;
using Data.Gtfs;

namespace Data.Feeds
{
    public static class Feeds
    {
        public static GtfsFeed Get(string feedId)
        {
            return new ZipFileGtfsFeed($"Feeds/{feedId}.zip");
        }
    }
}
