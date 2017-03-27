using Core.Gtfs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Bar
    {
        public DateTime Leave { get; set; }
        public DateTime Arrive { get; set; }
    }

    public class CommuteService
    {
        public IEnumerable<Bar> Get(GtfsFeed feed, DateTime now)
        {
            var serviceId = feed.GetApplicableServiceId(now);
            var trips = feed.Trips.Where(t => t.ServiceId == serviceId);
            var allStopTimes = feed.StopTimes.GroupBy(st => st.TripId).ToDictionary(g => g.Key, g => g.AsEnumerable());
            var tripsWithStop = trips.Where(t => allStopTimes[t.TripId].Any(st => st.StopId == "30913"));
            var stopTimesAtStop = tripsWithStop.Select(t => allStopTimes[t.TripId].Single(st => st.StopId == "30913"));

            return stopTimesAtStop.Select(st => new Bar
            {
                Leave = now.Date + st.DepartureTime
            }).OrderBy(b => b.Leave);
        }
    }
}
