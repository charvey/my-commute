using Core;
using Data.Feeds;
using System;
using System.Diagnostics;
using System.IO;

namespace Terminal
{
	class Program
	{
		static void Main(string[] args)
		{
			var feed = SeptaFeeds.Latest;
			var anyGivenMonday = new DateTime(2017, 9, 11);

			Console.WriteLine("To Work");
			using (var file = new FileStream("table.html", FileMode.Create))
			using (var writer = new StreamWriter(file))
			{
				writer.WriteLine("<html>");
				writer.WriteLine("<head>");
				writer.WriteLine("<link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' integrity='sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u' crossorigin='anonymous'>");
				writer.WriteLine("</head>");
				writer.WriteLine("<body>");

				writer.WriteLine("<table class='table table-striped table-condensed'>");
				writer.WriteLine("<thead>");
				writer.WriteLine("<tr>" +
					"<th colspan='2'>To Wissahickon</th>" +
					"<th colspan='2'>To Overbrook</th>" +
					"<th colspan='2'>To Radnor</th>" +
					"<th>Duration</th>" +
				"</tr>");
				writer.WriteLine("</thead>");
				writer.WriteLine("<tbody>");
				foreach (var st in CommuteService.ToWork(feed, anyGivenMonday))
				{
					writer.WriteLine("<tr>" +
						"<td>" + st[0].DepartureTime.ToTimeOfDay() + "</td><td>" + st[1].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + st[2].DepartureTime.ToTimeOfDay() + "</td><td>" + st[3].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + st[4].DepartureTime.ToTimeOfDay() + "</td><td>" + st[5].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + (st[5].ArrivalTime - st[0].DepartureTime).ToLengthOfTime() + "</td>" +
					"</tr>");

					Console.WriteLine(
						st[0].DepartureTime.ToTimeOfDay() + " " + st[1].ArrivalTime.ToTimeOfDay() + "\t" +
						st[2].DepartureTime.ToTimeOfDay() + " " + st[3].ArrivalTime.ToTimeOfDay() + "\t" +
						st[4].DepartureTime.ToTimeOfDay() + " " + st[5].ArrivalTime.ToTimeOfDay() + "\t" +
						(st[5].ArrivalTime - st[0].DepartureTime).ToLengthOfTime()
						);
				}
				writer.WriteLine("</tbody>");
				writer.WriteLine("</table>");

				Console.WriteLine("To Home");
				writer.WriteLine("<table class='table table-striped table-condensed'>");
				writer.WriteLine("<thead>");
				writer.WriteLine("<tr>" +
					"<th colspan='2'>To Radnor NHSL</th>" +
					"<th colspan='2'>To Norristown</th>" +
					"<th colspan='2'>To Manayunk</th>" +
					"<th>Duration</th>" +
				"</tr>");
				writer.WriteLine("</thead>");
				writer.WriteLine("<tbody>");
				foreach (var st in CommuteService.ToHome(feed, anyGivenMonday))
				{
					writer.WriteLine("<tr>" +
						"<td>" + st[0].DepartureTime.ToTimeOfDay() + "</td><td>" + st[1].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + st[2].DepartureTime.ToTimeOfDay() + "</td><td>" + st[3].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + st[4].DepartureTime.ToTimeOfDay() + "</td><td>" + st[5].ArrivalTime.ToTimeOfDay() + "</td>" +
						"<td>" + (st[5].ArrivalTime - st[0].DepartureTime).ToLengthOfTime() + "</td>" +
					"</tr>");

					Console.WriteLine(
						st[0].DepartureTime.ToTimeOfDay() + " " + st[1].ArrivalTime.ToTimeOfDay() + "\t" +
						st[2].DepartureTime.ToTimeOfDay() + " " + st[3].ArrivalTime.ToTimeOfDay() + "\t" +
						st[4].DepartureTime.ToTimeOfDay() + " " + st[5].ArrivalTime.ToTimeOfDay() + "\t" +
						(st[5].ArrivalTime - st[0].DepartureTime).ToLengthOfTime()
						);
				}
				writer.WriteLine("</tbody>");
				writer.WriteLine("</table>");
				writer.WriteLine("</body>");
				writer.WriteLine("</html>");
			}

			if (Debugger.IsAttached) Console.Read();
		}
	}

	internal static class TimeSpanExtensions
	{
		internal static string ToTimeOfDay(this TimeSpan time)
		{
			return (DateTime.Today + time).ToString("t");
		}

		internal static string ToLengthOfTime(this TimeSpan time)
		{
			if (time.TotalHours >= 1)
				return time.Hours + "h " + time.Minutes + "m";
			return time.Minutes + "m";
		}
	}
}
