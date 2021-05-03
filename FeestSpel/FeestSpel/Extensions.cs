using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeestSpel
{
    public static class Extensions
    {
        public static string GetStringValue(this ISession session, string key)
        {
            byte[] value = new byte[0];
            session.TryGetValue(key, out value);

            if (value != null && value.Length > 0)
                return Encoding.UTF8.GetString(value);

            return "";
        }

        public static void SetStringValue(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }
    }
}
