using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ASP_Test
{
    public static class MySessionExtensions 
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
            {
                return default(T);
            }
            var data = JsonConvert.DeserializeObject<T>(value);

            return data;
        }
    }
}
