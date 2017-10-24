using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resin.Api.Client.Interfaces;

namespace Resin.Api.Client
{
    internal static class ResinExtensions
    {
        public static string FormatJson(this string json)
        {
            try
            {
                return JToken.Parse(json).ToString(Formatting.Indented);
            }
            catch (Exception)
            {
                return json;
            }
        }

        public static TODataObject[] ToDataObjectArray<TODataObject>(this JToken token, ApiClientBase client)
            where TODataObject : IDeferrableObject, new()
        {
            JToken d = token["d"];

            if (d.Type != JTokenType.Array)
                throw new InvalidOperationException();

            return d.Children()
                .Select(c =>
                {
                    var o = new TODataObject();

                    o.Initialize(client, c);

                    return o;

                }).ToArray();
        }

        public static TODataObject ToDataObject<TODataObject>(this JToken token, ApiClientBase client)
            where TODataObject : IDeferrableObject, new()
        {
            JToken d = token["d"];

            var obj = new TODataObject();

            obj.Initialize(client, d);

            return obj;
        }

        /// <summary>
        /// Skips the "d" level of referencing.
        /// </summary>
        /// <typeparam name="TODataObject"></typeparam>
        /// <param name="token"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static TODataObject ToDataObjectDirect<TODataObject>(this JToken token, ApiClientBase client)
            where TODataObject : IDeferrableObject, new()
        {
            var obj = new TODataObject();

            obj.Initialize(client, token);

            return obj;
        }
    }
}