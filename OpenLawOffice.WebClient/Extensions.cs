using System;
using System.IO;
using Newtonsoft.Json;

namespace OpenLawOffice.WebClient
{
    public static class Extensions
    {
        public static T JsonDeserialize<T>(this Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sr = new StreamReader(stream))
            {
                string str = sr.ReadToEnd();
                T obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
                //using (JsonTextReader jtr = new JsonTextReader(sr))
                //{
                //    return serializer.Deserialize<T>(jtr);
                //}
            }
        }
    }
}