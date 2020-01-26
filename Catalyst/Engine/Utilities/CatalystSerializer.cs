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
        Xml
    }

    public static class CatalystSerializer
    {
        public static void SerializeToFile<T>(T obj, string filepath, SerializationMode mode)
        {

            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                if (mode == SerializationMode.Binary)
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fileStream, obj);
                }
                else if (mode == SerializationMode.Xml)
                {
                    var xs = new XmlSerializer(typeof(T));
                    xs.Serialize(fileStream, obj);
                }
            }

        }

        public static T DeserializeFromFile<T>(string filepath, SerializationMode mode)
        {
            T data;
            using (var fileStream = File.OpenRead(filepath))
            {

                //Deserialize
                if (mode == SerializationMode.Binary)
                {
                    var bf = new BinaryFormatter();
                    data = (T)bf.Deserialize(fileStream);
                }
                else
                {
                    var xs = new XmlSerializer(typeof(T));
                    data = (T)xs.Deserialize(fileStream);
                }
            }

            return data;
        }


    }
}
