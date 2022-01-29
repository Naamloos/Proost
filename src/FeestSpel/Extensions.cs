using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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

        public static string GetStringValue(this IFormCollection form, string key)
        {
            form.TryGetValue(key, out StringValues value);

            if (value.Count() > 0)
                return value;

            return "";
        }

        public static string[] GetStringArray(this IFormCollection form, string key)
        {
            form.TryGetValue(key, out StringValues value);

            return value.Select(x => x.ToString()).ToArray();
        }

        public static List<string> GetPlayers(this IFormCollection form)
        {
            return form.Keys.Where(x => x.StartsWith("player")).Select(x => form.GetStringValue(x)).ToList();
        }

        public static void SetStringValue(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }
    }
}
