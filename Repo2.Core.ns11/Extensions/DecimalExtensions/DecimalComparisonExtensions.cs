using System;

namespace Repo2.Core.ns11.Extensions.DecimalExtensions
{
    public static class DecimalComparisonExtensions
    {
        public static bool AlmostEqualTo(this decimal value1, decimal value2, decimal tolerance = 0.0000001M)
            => Math.Abs(value1 - value2) <= tolerance;


        public static bool AlmostEqualTo(this double value1, double value2, decimal tolerance = 0.0000001M)
            => ((decimal)value1).AlmostEqualTo((decimal)value2, tolerance);


        public static bool AlmostEqualTo(this double value1, decimal value2, decimal tolerance = 0.0000001M)
            => ((decimal)value1).AlmostEqualTo(value2, tolerance);


        public static bool AlmostEqualTo(this decimal value1, double value2, decimal tolerance = 0.0000001M)
            => (value1).AlmostEqualTo((decimal)value2, tolerance);
    }
}
