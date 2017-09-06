using Core.Gtfs;
using Data.Gtfs;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace Data.Feeds
{
	public static class SeptaFeeds
	{
		public static GtfsFeed Latest
		{
			get
			{				
				DownloadLatestFeedFiles();
				return new AggregateGtfsFeed(new[] {
					new ZipFileGtfsFeed("gtfs_public/google_rail.zip"),
					new ZipFileGtfsFeed("gtfs_public/google_bus.zip")
				});
			}
		}

		private static void DownloadLatestFeedFiles()
		{
			const string latestUrl = "https://github.com/septadev/GTFS/releases/download/v20170903/gtfs_public.zip";

			if (!File.Exists("gtfs_public/google_rail.zip"))
			{
				if (!File.Exists("gtfs_public.zip"))
					Download(latestUrl, "gtfs_public.zip");

				Extract("gtfs_public.zip");
			}

			if (!File.Exists("gtfs_public/google_bus.zip"))
			{
				if (!File.Exists("gtfs_public.zip"))
					Download(latestUrl, "gtfs_public.zip");

				Extract("gtfs_public.zip");
			}
		}

		private static void Extract(string zipPath)
		{
			using (var file = ZipFile.OpenRead(zipPath))
			{
				var dest = Path.Combine(Path.GetDirectoryName(zipPath), Path.GetFileNameWithoutExtension(zipPath));
				if (Directory.Exists(dest)) Directory.Delete(dest, true);
				file.ExtractToDirectory(dest);
			}
		}

		private static HttpClient httpClient = new HttpClient();
		internal static void Download(string address, string fileName)
		{
			using (var fileStream = new FileStream(fileName, FileMode.Create))
			{
				httpClient.GetStreamAsync(address).Result.CopyTo(fileStream);
			}
		}
	}
}
