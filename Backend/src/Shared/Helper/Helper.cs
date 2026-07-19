using Newtonsoft.Json;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Helper
{
    public static class Helper
    {
        /// <summary>
        /// Deserialize json into T object, extracts every single property out of json and maps it into T object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public static T FallbackDeserializer<T>(string body) where T : new()
        {
            var result = new T();

            // on bad json to not catch ex and preserve anything that possible.
            var partialResult = JsonConvert.DeserializeObject<T>(body, new JsonSerializerSettings
            {
                Error = (_, args) => args.ErrorContext.Handled = true
            });

            if (partialResult == null) return result;

            foreach (var property in typeof(T).GetProperties())
            {
                var propValue = property.GetValue(partialResult);

                if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty(propValue as string))
                        property.SetValue(result, propValue);
                }
                else if (property.PropertyType.IsEnum)
                {
                    var defaultEnumValue = Activator.CreateInstance(property.PropertyType);
                    if (!Equals(propValue, defaultEnumValue))
                        property.SetValue(result, propValue);
                }
                else
                {
                    if (propValue != null) property.SetValue(result, propValue);
                }
            }
            return result;
        }
    }
}
