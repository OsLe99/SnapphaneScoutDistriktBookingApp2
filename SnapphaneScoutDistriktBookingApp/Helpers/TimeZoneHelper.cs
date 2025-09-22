using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Helpers
{
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo SwedishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");

        public static DateTime ToSwedishTime(DateTime utcDateTime) => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, SwedishTimeZone);

        public static DateTime FromSwedishTime(DateTime localDateTime) => TimeZoneInfo.ConvertTimeToUtc(localDateTime, SwedishTimeZone);
    }
}
