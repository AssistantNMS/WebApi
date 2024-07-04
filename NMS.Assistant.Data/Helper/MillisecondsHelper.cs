namespace NMS.Assistant.Data.Helper
{
    public static class MillisecondsHelper
    {
        public static int FromSeconds(int secs)
        {
            return secs * 1000;
        }
        public static int FromMinutes(int min)
        {
            int secs = min * 60;
            return FromSeconds(secs);
        }
    }
}
