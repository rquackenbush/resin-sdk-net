using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}