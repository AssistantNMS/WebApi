namespace NMS.Assistant.Data.Helper
{
    public static class SecondsHelper
    {
        public static int FromMinutes(int min)
        {
            return min * 60;
        }

        public static int FromHours(int hours)
        {
            int min = hours * 60;
            return FromMinutes(min);
        }

        public static int FromDays(int days)
        {
            int hours = days * 24;
            return FromHours(hours);
        }

        public static int FromMonths(int months)
        {
            int days = months * 31;
            return FromDays(days);
        }
    }
}
