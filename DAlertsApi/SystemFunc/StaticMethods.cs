using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.SystemFunc
{
    public class StaticMethods
    {
        public static string GetUrl(string url, string port = null, string routing = null)
        { 
            Uri uri = new Uri(url + routing);
            if (port == null) return url;
            var builder = new UriBuilder(uri)
            {
                Port = int.Parse(port)
            };
            return builder.ToString();
        }

        public static IEnumerable<KeyValuePair<string, string>> FromClassToDictionary(object obj)
        {
            return obj
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(e => e.GetValue(obj) != null)
                    .Select(e => new KeyValuePair<string, string>(
                        e.Name.ToLower(),
                        e.GetValue(obj)?.ToString() ?? string.Empty));
        }
    }
}
