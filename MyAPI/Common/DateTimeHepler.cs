using System;

namespace MyAPI.Common
{
    public class DateTimeHepler
    {
        public static long ConvertToTimeStamp(DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            return (long)(dateTime - startTime).TotalSeconds;
        }

        public static DateTime ConvertToDateTime(long timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            return startTime.AddSeconds(timeStamp);
        }
    }
}