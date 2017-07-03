using Core.Gtfs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Foo
    {
        public DateTime Leave => Segments.First().Leave;
        public Bar[] Segments { get; set; }
        public DateTime Arrive => Segments.Last().Arrive;
    }

    public class Bar
    {
        public DateTime Leave { get; set; }
        public DateTime Arrive { get; set; }
    }

    public class CommuteService
    {
		public IEnumerable<Foo> Get(GtfsFeed feed, DateTime now)
		{
			IEnumerable<StopTime[]> result;
			if (now.TimeOfDay <= TimeSpan.Parse("12:30"))
				result = ToWork(feed, now);
			else
				result = ToHome(feed, now);

			return result.Select(x => new Foo
			{
				Segments = new[]
				{
					new Bar{Leave=now.Date.Add(x[0].DepartureTime),Arrive=now.Date.Add(x[1].ArrivalTime)},
					new Bar{Leave=now.Date.Add(x[2].DepartureTime),Arrive=now.Date.Add(x[3].ArrivalTime)},
					new Bar{Leave=now.Date.Add(x[4].DepartureTime),Arrive=now.Date.Add(x[5].ArrivalTime)},
				}
			}).Where(x => x.Leave >= now);
		}

		public static IEnumerable<StopTime[]> ToWork(GtfsFeed feed, DateTime now)
		{
			var serviceIds = new HashSet<string>(feed.GetApplicableServiceIds(now));

			var startingLegs = GetDepartureTimes(feed, serviceIds, "30913", "30596", TimeSpan.Zero, TimeSpan.FromHours(14))
				.OrderBy(z => z.Item1.DepartureTime).ToArray();

			foreach (var startingLeg in startingLegs)
			{
				var nextLeg = GetDepartureTimes(feed, serviceIds, "30596", "20568", startingLeg.Item2.ArrivalTime, TimeSpan.FromHours(24))
					.OrderBy(x => x.Item2.ArrivalTime).First();
				var finalLeg = GetDepartureTimes(feed, serviceIds, "90522", "90513", nextLeg.Item2.ArrivalTime + TimeSpan.FromMinutes(2), TimeSpan.FromHours(24))
					.OrderBy(x => x.Item2.ArrivalTime).First();

				yield return new StopTime[]
				{
					startingLeg.Item1,
					startingLeg.Item2,
					nextLeg.Item1,
					nextLeg.Item2,
					finalLeg.Item1,
					finalLeg.Item2
				};
			}
		}

		public static IEnumerable<StopTime[]> ToHome(GtfsFeed feed, DateTime now)
		{
			var serviceIds = new HashSet<string>(feed.GetApplicableServiceIds(now));

			var startingLegs = new[] { "14:59", "15:14", "15:29", "15:44", "15:59", "16:14", "16:29", "16:44", "16:59", "17:14", "17:29", "17:59", "18:09", "18:24", "18:39" }
				.Select(TimeSpan.Parse)
				.Select(t => Tuple.Create(new StopTime { DepartureTime = t }, new StopTime { ArrivalTime = t.Add(TimeSpan.FromMinutes(10)) }))
				.ToList();

			foreach (var startingLeg in startingLegs)
			{
				var nextLeg = GetDepartureTimes(feed, serviceIds, "1930", "30520", startingLeg.Item2.ArrivalTime, TimeSpan.FromHours(24))
					.OrderBy(x => x.Item2.ArrivalTime).First();
				var finalLeg = GetDepartureTimes(feed, serviceIds, "90226", "90221", nextLeg.Item2.ArrivalTime + TimeSpan.FromMinutes(2), TimeSpan.FromHours(24))
					.OrderBy(x => x.Item2.ArrivalTime).First();

				yield return new StopTime[]
				{
					startingLeg.Item1,
					startingLeg.Item2,
					nextLeg.Item1,
					nextLeg.Item2,
					finalLeg.Item1,
					finalLeg.Item2
				};
			}
		}

		static Dictionary<GtfsFeed, Dictionary<string, List<StopTime>>> cachedStopTimes = new Dictionary<GtfsFeed, Dictionary<string, List<StopTime>>>();

		static IEnumerable<Tuple<StopTime, StopTime>> GetDepartureTimes(GtfsFeed feed, ISet<string> serviceIds,
			string fromStopId, string toStopId,
			TimeSpan startTime, TimeSpan endTime)
		{
			if (!cachedStopTimes.ContainsKey(feed))
				cachedStopTimes[feed] = feed.StopTimes
				.GroupBy(st => st.TripId)
				.ToDictionary(x => x.Key, x => x.OrderBy(st => st.StopSequence).ToList());
			var stopTimes = cachedStopTimes[feed];

			var trips = feed.Trips
				.Where(t => serviceIds.Contains(t.ServiceId))
				.Where(t => stopTimes[t.TripId].Any(st => st.StopId == fromStopId))
				.Where(t => startTime <= stopTimes[t.TripId].Single(st => st.StopId == fromStopId).DepartureTime)
				.Where(t => stopTimes[t.TripId].Single(st => st.StopId == fromStopId).DepartureTime <= endTime)
				.Where(t => stopTimes[t.TripId].Any(st => st.StopId == toStopId))
				.Where(t => stopTimes[t.TripId].Single(st => st.StopId == fromStopId).StopSequence < stopTimes[t.TripId].Single(st => st.StopId == toStopId).StopSequence);

			return trips.Select(t => Tuple.Create(
				stopTimes[t.TripId].Single(st => st.StopId == fromStopId),
				 stopTimes[t.TripId].Single(st => st.StopId == toStopId)));
		}
    }
}
