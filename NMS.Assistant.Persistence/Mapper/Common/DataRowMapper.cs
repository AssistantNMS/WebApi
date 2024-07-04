using System;
using System.Data;

namespace NMS.Assistant.Persistence.Mapper.Common
{
    public static class DataRowMapper
    {
        public static string ReadString(this DataRow row, string prop)
        {
            try
            {
                return row[prop].ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Guid ReadGuid(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = Guid.TryParse(stringResult, out Guid result);
            return parseSuccess ? result : Guid.Empty;
        }

        public static int ReadInt(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = int.TryParse(stringResult, out int result);
            return parseSuccess ? result : 0;
        }

        public static long ReadLong(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = long.TryParse(stringResult, out long result);
            return parseSuccess ? result : 0;
        }

        public static DateTime ReadDateTime(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = DateTime.TryParse(stringResult, out DateTime result);
            return parseSuccess ? result : DateTime.MinValue;
        }

        public static bool ReadBool(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = bool.TryParse(stringResult, out bool result);
            return parseSuccess && result;
        }

        public static TEnum ReadEnum<TEnum>(this DataRow row, string prop) where TEnum : struct
        {
            int intResult = row.ReadInt(prop);

            if (!typeof(TEnum).IsEnum) return default;
            return (TEnum)(object)intResult;
        }

        public static decimal ReadDecimal(this DataRow row, string prop)
        {
            string stringResult = row.ReadString(prop);

            bool parseSuccess = decimal.TryParse(stringResult, out decimal result);
            return parseSuccess ? result : 0;
        }
    }
}
