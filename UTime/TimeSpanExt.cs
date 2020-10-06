using System;

namespace Utils.UTime
{
    public static class TimeSpanExt
    {
        public static TimeSpan Multiply(this TimeSpan @this, int multiplier) =>
            TimeSpan.FromTicks(@this.Ticks * multiplier);

        public static TimeSpan Multiply(this TimeSpan @this, float multiplier) =>
            TimeSpan.FromTicks((long)(@this.Ticks * multiplier));
    }
}
