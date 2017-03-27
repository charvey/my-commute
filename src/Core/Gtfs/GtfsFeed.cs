using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Gtfs
{
    public interface GtfsFeed
    {
        IEnumerable<Calendar> Calendars { get; }
        IEnumerable<CalendarDate> CalendarDates { get; }
        IEnumerable<StopTime> StopTimes { get; }
        IEnumerable<Trip> Trips { get; }
    }

    public static class GtfsFeedExtensions
    {
        public static string GetApplicableServiceId(this GtfsFeed feed, DateTime now)
        {
            return feed.Calendars
                .Where(c => c.StartDate <= now && now <= c.EndDate)
                .Where(c => CalendarApplies(now.DayOfWeek, c))
                .Select(c => c.ServiceId)
                .Except(feed.CalendarDates.Where(c => c.Date == now.Date && c.ExceptionType == ExceptionType.Remove).Select(c => c.ServiceId))
                .Concat(feed.CalendarDates.Where(c => c.Date == now.Date && c.ExceptionType == ExceptionType.Add).Select(c => c.ServiceId))
                .Distinct().Single();
        }

        private static bool CalendarApplies(DayOfWeek dayOfWeek, Calendar calendar)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday: return calendar.Sunday;
                case DayOfWeek.Monday:return calendar.Monday;
                case DayOfWeek.Tuesday: return calendar.Tuesday;
                case DayOfWeek.Wednesday: return calendar.Wednesday;
                case DayOfWeek.Thursday: return calendar.Thursday;
                case DayOfWeek.Friday: return calendar.Friday;
                case DayOfWeek.Saturday: return calendar.Saturday;
                default: throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }
        }
    }
}
