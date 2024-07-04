using System;

namespace NMS.Assistant.Domain.Helper
{
    public static class DateHelper
    {
        public static DateTime EpochDate => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime GetFrontendSafeDateTime(DateTime date)
        {
            try
            {
                return DateTime.Parse($"{date.Year}-{date.Month.DatePad()}-{date.Day.DatePad()}T{date.Hour.DatePad()}:{date.Minute.DatePad()}:{date.Second.DatePad()}");
            }
            catch (Exception)
            {
                return date;
            }
        }

        public static DateTime GetFrontendSafeDateTime(string dateString)
        {
            bool dateConvert = DateTime.TryParse(dateString, out DateTime date);
            if (dateConvert == false) return DateTime.MinValue;

            return GetFrontendSafeDateTime(date);
        }

        public static string DatePad(this int number)
        {
            string temp = number.ToString();
            return temp.Length != 2 ? $"0{temp}" : temp;
        }

        public static DateTime GetFrontendSafeDateTimeNow() => GetFrontendSafeDateTime(DateTime.Now);

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = EpochDate;
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime MinutesSinceEpochToDateTime(double minutes, DateTime? date = null)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = date ?? EpochDate;
            dtDateTime = dtDateTime.AddMinutes(minutes);
            return dtDateTime;
        }

        public static DateTime HoursSinceEpochToDateTime(double hours, DateTime? date = null)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = date ?? EpochDate;
            dtDateTime = dtDateTime.AddHours(hours);
            return dtDateTime;
        }
    }
}
