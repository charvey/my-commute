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
            var stops = new[]
            {
                "30913", // Main & Levering
                "30596", //"31032", // Wissahickon
                "20568", // Near Overbrook
                "90522", // Overbrook Station
                "90513" // Radnor Station
            };

            return Get(feed, now, stops[0], stops[1])
                .Select(o => new Foo
                {
                    Segments = new[] { o }
                    .Concat(Get(feed, o.Arrive, stops[1], stops.Skip(1).ToArray()))
                    .ToArray()
                });
        }

        private Dictionary<GtfsFeed, Dictionary<string, Dictionary<string, StopTime[]>>> stopTimes = new Dictionary<GtfsFeed, Dictionary<string, Dictionary<string, StopTime[]>>>();

        private Bar[] Get(GtfsFeed feed, DateTime now, string current, string[] remaining)
        {
            if (remaining.Length == 0)
            {
                return new Bar[0];
            }

            var next = Get(feed, now, current, remaining[0]).First();
            return new Bar[] { next }
            .Concat(Get(feed, next.Arrive, remaining[0], remaining.Skip(1).ToArray()))
            .ToArray();
        }

        private IEnumerable<Bar> Get(GtfsFeed feed, DateTime now, string from, string to)
        {
            if (!stopTimes.ContainsKey(feed))
                stopTimes[feed] = feed.StopTimes
                    .GroupBy(st => st.TripId)
                    .ToDictionary(
                        tg => tg.Key,
                        tg => tg
                            .GroupBy(st => st.StopId)
                            .ToDictionary(
                                stg => stg.Key,
                                stg => stg
                                    .AsEnumerable()
                                    .OrderBy(st => st.StopSequence)
                                    .ToArray()
                            )
                        );

            var serviceIds = new HashSet<string>(feed.GetApplicableServiceIds(now));
            return feed.Trips
                .Where(t => serviceIds.Contains(t.ServiceId))
                .Where(t =>
                {
                    return stopTimes[feed][t.TripId].ContainsKey(from)
                        && stopTimes[feed][t.TripId].ContainsKey(to);
                })
                .Select(t =>
                {
                    var tripStopTimes = stopTimes[feed][t.TripId];

                    var fromStop = tripStopTimes[from]
                        .SkipWhile(st => now.Date + st.DepartureTime < now)
                        .FirstOrDefault();

                    if (fromStop == null) return null;

                    var toStop = tripStopTimes[to]
                        .SkipWhile(st => st.StopSequence < fromStop.StopSequence)
                        .FirstOrDefault();

                    if (toStop == null) return null;

                    return new TripStopTimePair
                    {
                        Trip = t,
                        From = fromStop,
                        To = toStop
                    };
                }).Where(x => x != null)
                .Select(t => new Bar
                {
                    Leave = now.Date + t.From.DepartureTime,
                    Arrive = now.Date + t.To.ArrivalTime
                }).OrderBy(b => b.Leave)
                .Concat(new[]{
                    new Bar{Leave=now,Arrive=now}//Walk
                });
        }

        private class TripStopTimePair
        {
            public Trip Trip;
            public StopTime From;
            public StopTime To;
        }
    }
}
