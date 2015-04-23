using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System
{
    public static class TimeSpanExtensions
    {
        public static string ToDurationString(this TimeSpan duration, bool alwaysShowSeconds = false)
        {
            if (duration == TimeSpan.MaxValue)
                return "Infinite";

            if (duration == TimeSpan.MinValue)
                return "N/A";

            StringBuilder sb = new StringBuilder();
            
            if (duration.Days > 0)
            {
                sb.Append(duration.Days);
                sb.Append(" z");
            }

            if (duration.Hours > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(duration.Hours);
                sb.Append(" h");
            }

            if (duration.Minutes > 0)
            {
                if(sb.Length > 0)
                   sb.Append(", ");
                sb.Append(duration.Minutes);
                sb.Append(" min");
            }

            if (duration.Seconds > 0)
            {
                if (duration.TotalMinutes < 10 || alwaysShowSeconds)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(duration.Seconds);
                    sb.Append(" s");
                }
            }

            return sb.ToString();
        }
    }
}