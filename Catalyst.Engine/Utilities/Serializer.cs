using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Catalyst.Engine.Utilities
{

    public enum SerializationMode
    {
        Binary,
        Json
    }

    public static class Serializer
    {
        public static void SerializeToFile<T>(T obj, string filepath, SerializationMode mode)
        {
            if (mode == SerializationMode.Binary)
            {
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {

                    var bf = new BinaryFormatter();
                    bf.Serialize(fileStream, obj);
                }
            }
            else if (mode == SerializationMode.Json)
            {
                string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize });
                File.WriteAllText(filepath, json);
            }


        }

        public static T DeserializeFromFile<T>(string filepath, SerializationMode mode)
        {
            T data;
            if (mode == SerializationMode.Binary)
            {
                using (var fileStream = File.OpenRead(filepath))
                {

                    //Deserialize

                    var bf = new BinaryFormatter();
                    data = (T)bf.Deserialize(fileStream);
                }
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath), new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize });
            }


            return data;
        }


    }
}
