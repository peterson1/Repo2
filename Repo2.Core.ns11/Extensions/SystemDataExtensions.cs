using System;
using System.Data;

namespace Repo2.Core.ns11.Extensions
{
    public static class SystemDataExtensions
    {

        public static int? ToInt_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetInt32(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                int num = 0;
                if (int.TryParse(val.ToString(), out num))
                    return num;
                else
                    return null;
            }
        }

        public static int ToInt(this IDataRecord rec, int i, int defaultVal = 0)
            => ToInt_(rec, i) ?? defaultVal;


        public static long? ToLong_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetInt64(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                long num = 0;
                if (long.TryParse(val.ToString(), out num))
                    return num;
                else
                    return null;
            }
        }


        public static long ToLong(this IDataRecord rec, int i, long defaultVal = 0)
            => ToLong_(rec, i) ?? defaultVal;


        public static double? ToDouble_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetDouble(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                double num = 0;
                if (double.TryParse(val.ToString(), out num))
                    return num;
                else
                    return null;
            }
        }


        public static decimal? ToDecimal_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetDecimal(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                decimal num = 0;
                if (decimal.TryParse(val.ToString(), out num))
                    return num;
                else
                    return null;
            }
        }

        public static decimal ToDecimal(this IDataRecord rec, int i, decimal defaultVal = 0)
            => ToDecimal_(rec, i) ?? defaultVal;



        public static bool? ToBool_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetBoolean(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                bool parsd;
                if (bool.TryParse(val.ToString(), out parsd))
                    return parsd;
                else
                    return null;
            }
        }


        public static bool ToBool(this IDataRecord rec, int i, bool defaultValue = false)
            => rec.ToBool_(i) ?? defaultValue;


        public static short? ToShort_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetInt16(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                short num = 0;
                if (short.TryParse(val.ToString(), out num))
                    return num;
                else
                    return null;
            }
        }


        public static char? ToChar_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return rec.GetChar(i);
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                char chr;
                if (char.TryParse(val.ToString(), out chr))
                    return chr;
                else
                    return null;
            }
        }


        public static DateTime? ToDate_(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            try
            {
                return new DateTime?(rec.GetDateTime(i));
            }
            catch (InvalidCastException)
            {
                var val = rec.GetValue(i);
                if (val == null) return null;

                DateTime date;
                if (DateTime.TryParse(val.ToString(), out date))
                    return new DateTime?(date);
                else
                    return null;
            }
        }

        public static DateTime ToDate(this IDataRecord rec, int i)
            => ToDate_(rec, i) ?? DateTime.MinValue;



        public static string ToText(this IDataRecord rec, int i)
        {
            if (rec.IsDBNull(i)) return null;
            var val = rec.GetValue(i);
            return val?.ToString();
        }

    }
}
