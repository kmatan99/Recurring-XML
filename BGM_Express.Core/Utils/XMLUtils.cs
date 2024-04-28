using BGM_Express.Core.Dto;
using System.Xml.Serialization;

namespace BGM_Express.Core.Utils
{
    public static class XMLUtils
    {
        public static T DeserializeXML<T>(MemoryStream stream) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(stream);

            T deserializedFile = (T)xmlSerializer.Deserialize(textReader);

            return deserializedFile;
        }

        public static string SerializeXML<T>(DispatchOrderDto dispatchOrder)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, dispatchOrder);

                return writer.ToString();
            }
        }

        public static void SaveXmlToFile(string xmlContent, string fileName, string filePath)
        {
            StreamWriter file = new StreamWriter($"{filePath}\\{fileName}");

            file.WriteLine(xmlContent);
            file.Close();
        }
    }
}
