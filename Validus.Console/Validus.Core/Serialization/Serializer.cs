using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

using Newtonsoft.Json; // TODO: JSON serializer ?

namespace Validus.Core.Serialization
{
    public static class Serializer
    {
        public static string SerializeObject(object serializableObject)
        {
            var serializedObject = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, serializableObject);

                serializedObject = Convert.ToBase64String(memoryStream.GetBuffer());
            }

            return serializedObject;
        }

        public static object DeserializeObject(string serializedObject)
        {
            object deserializedObject = null;

            var serializedData = Convert.FromBase64String(serializedObject);

            using (var memoryStream = new MemoryStream(serializedData))
            {
                var binaryFormatter = new BinaryFormatter();

                deserializedObject = binaryFormatter.Deserialize(memoryStream);
            }

            return deserializedObject;
        }

        public static object XMLDeserializeObject(string serializedObject, Type serializableType)
        {
            object deserializedObject = null;

            if (string.IsNullOrEmpty(serializedObject) == false)
            {
                var xmlSerializer = new XmlSerializer(serializableType);

                using (var xmlReader = new StringReader(serializedObject))
                {
                    deserializedObject = xmlSerializer.Deserialize(xmlReader);
                }
            }

            return deserializedObject;
        }

        public static string XMLSerializeObject(object serializableObject, Type serializableType)
        {
            var serializedObject = string.Empty;

            if (serializableObject != null)
            {
                var xmlSerializer = new XmlSerializer(serializableType);

                using (var xmlWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(xmlWriter, serializableObject);

                    serializedObject = xmlWriter.ToString();
                }
            }

            return serializedObject;
        }
    }
}