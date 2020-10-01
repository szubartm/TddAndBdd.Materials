using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.TestUtils
{
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public string KeyName = "key";
        public string ValueName = "value";

        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict) { }


        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema() => null;

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey), null, null, new XmlRootAttribute(KeyName), null);
            var valueSerializer = new XmlSerializer(typeof(TValue), null, null, new XmlRootAttribute(ValueName), null);

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                var key = (TKey)keySerializer.Deserialize(reader);
                var value = (TValue)valueSerializer.Deserialize(reader);
                Add(key, value);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in Keys)
            {
                keySerializer.Serialize(writer, key);
                valueSerializer.Serialize(writer, this[key]);
            }
        }
        #endregion
    }
}