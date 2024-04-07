using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Text.Json;
namespace main_prj.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson (this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetJson<T> (this ISession session, string key)
        {
            var sessionData= session.GetString(key);
            return sessionData == null ?
                default(T) : JsonSerializer.Deserialize<T> (sessionData);
        }
    }
}
