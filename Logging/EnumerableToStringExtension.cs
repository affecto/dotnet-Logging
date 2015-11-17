using System.Collections.Generic;
using System.Text;

namespace Affecto.Logging
{
    public static class EnumerableToStringExtension
    {
        public static string ToLineSeparatedString<T>(this IEnumerable<T> list) where T : class
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                if (item != null)
                {
                    sb.AppendLine(item.ToString());
                }
            }
            return sb.ToString();
        }
    }
}