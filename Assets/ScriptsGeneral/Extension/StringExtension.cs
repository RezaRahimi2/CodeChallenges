using Newtonsoft.Json;

namespace Immersed.General
{
    public static class StringExtension
    {
        public static string FormatJson(this string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        public static string URLAntiCacheRandomizer(this string url)
        {
            string r = "";
            r += UnityEngine.Random.Range(
                1000000, 8000000).ToString();
            r += UnityEngine.Random.Range(
                1000000, 8000000).ToString();
            string result = url + "?p=" + r;
            return result;
        }
    }
}