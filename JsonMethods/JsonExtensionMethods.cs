using Newtonsoft.Json.Linq;
using System;

namespace JsonMethods
{
    /// <summary>
    /// Extends the methods of JSON objects.
    /// </summary>
    public static class JsonExtensionMethods
    {
        /// <summary>
        /// Determines whether the JSON object has the specified property name.<br/>
        /// Also specifies the culture, case and sort rules to be used.
        /// </summary>
        /// <param name="jObject">The <see cref="object"/> whose method gets extended.</param>
        /// <param name="propertyName">The name of the <see cref="JProperty"/> we are looking for in a <see cref="string"/> format.</param>
        /// <param name="stringComparison">Specifies the culture, case and sort rules to be used.</param>
        /// <returns><see langword="true"/> if the JSON object has the specified property; otherwise, <see langword="false"/></returns>
        public static bool ContainsKey(this JObject jObject, string propertyName, StringComparison stringComparison)
        {
            if (jObject?.GetValue(propertyName, stringComparison) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}