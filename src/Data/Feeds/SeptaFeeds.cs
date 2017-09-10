using Core.Gtfs;
using Data.Gtfs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
			var json = httpClient.GetStringAsync("https://api.github.com/repos/septadev/GTFS/releases/latest").Result;
			var release = JsonConvert.DeserializeObject<Release>(json);
			var asset = release.assets.Single();

			if (!File.Exists(asset.name) || DateTime.Parse(asset.updated_at) > new FileInfo(asset.name).LastWriteTime)
			{
				File.Delete(asset.name);
				Directory.Delete(Path.GetFileNameWithoutExtension(asset.name), true);
				Download(asset.browser_download_url, asset.name);
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

	public class Asset
	{
		public string name;
		public string updated_at { get; set; }
		public string browser_download_url { get; set; }
	}

	public class Release
	{
		public List<Asset> assets { get; set; }
	}
}
