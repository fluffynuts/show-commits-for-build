using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace get_build_log
{
    public static class JObjectExtensions
    {
        private static readonly JTokenResolvers _resolvers = new JTokenResolvers
        {
            {JTokenType.None, o => null},
            {JTokenType.Array, ConvertJTokenArray},
            {JTokenType.Property, ConvertJTokenProperty},
            {JTokenType.Integer, o => o.Value<int>()},
            {JTokenType.String, o => o.Value<string>()},
            {JTokenType.Boolean, o => o.Value<bool>()},
            {JTokenType.Null, o => null},
            {JTokenType.Undefined, o => null},
            {JTokenType.Date, o => o.Value<DateTime>()},
            {JTokenType.Bytes, o => o.Value<byte[]>()},
            {JTokenType.Guid, o => o.Value<Guid>()},
            {JTokenType.Uri, o => o.Value<Uri>()},
            {JTokenType.TimeSpan, o => o.Value<TimeSpan>()},
            {JTokenType.Object, TryConvertObject}
        };

        // just because JsonCovert doesn't believe in using your provided type all the way down >_<
        public static Dictionary<string, object> ToDictionary(this JObject src)
        {
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (src == null)
                return result;
            foreach (var prop in src.Properties())
                result[prop.Name] = _resolvers[prop.Type](prop.Value);
            return result;
        }

        private static object TryConvertObject(JToken arg)
        {
            var asJObject = arg as JObject;
            if (asJObject != null)
                return asJObject.ToDictionary();
            return PassThrough(arg);
        }

        private static object PassThrough(JToken arg)
        {
            return arg;
        }

        private static object ConvertJTokenProperty(JToken arg)
        {
            Func<JToken, object> resolver;
            if (_resolvers.TryGetValue(arg.Type, out resolver))
                return resolver(arg);
            throw new InvalidOperationException($"Unable to handle JToken of type: {arg.Type}");
        }

        private static object ConvertJTokenArray(JToken arg)
        {
            var array = arg as JArray;
            if (array == null)
                throw new NotImplementedException();
            var result = new List<object>();
            foreach (var item in array)
            {
                result.Add(TryConvertObject(item));
            }
            return result.ToArray();
        }

        private class JTokenResolvers : Dictionary<JTokenType, Func<JToken, object>>
        {
        }
    }
}