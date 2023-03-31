using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Common.TestUtils;

namespace UnitTesting.Stubs
{
    public class MockMethodProvider<TKey, TValue>
    {
        protected readonly System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> InnerDictionary;

        public MockMethodProvider(IDictionary<TKey, TValue> dict) => InnerDictionary = new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(dict);

        public IEnumerable<TKey> GetKeys() => InnerDictionary.Keys;

        public TValue GetValue(TKey key) => InnerDictionary[key];

        public void SaveAnywhere(Action<string> stringSaveAction)
        {
            var text = SaveToString();
            stringSaveAction(text);
        }
        public string SaveToString()
        {
            using var writer = new StringWriter();
            SaveToText(writer);
            writer.Flush();
            return writer.ToString();
        }
        public void SaveToStream(Stream stream)
        {
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            SaveToText(writer);
        }
        public void SaveToText(TextWriter writer)
        {
            var serDic = new SerializableDictionary<TKey, TValue>(InnerDictionary);
            var ser = new XmlSerializer(serDic.GetType());
            ser.Serialize(writer, serDic);
        }

        public static MockMethodProvider<TKey, TValue> LoadAnyhow(Func<string> stringLoadAction)
        {
            var text = stringLoadAction();
            return LoadFromString(text);
        }
        public static MockMethodProvider<TKey, TValue> LoadFromString(string @string)
        {
            using var reader = new StringReader(@string);
            return LoadFromText(reader);
        }
        public static MockMethodProvider<TKey, TValue> LoadFromStream(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return LoadFromText(reader);
        }
        public static MockMethodProvider<TKey, TValue> LoadFromText(TextReader reader)
        {
            var ser = new XmlSerializer(typeof(SerializableDictionary<TKey, TValue>));
            return new MockMethodProvider<TKey, TValue>(
              (SerializableDictionary<TKey, TValue>)ser.Deserialize(reader)
              );
        }
    }
}
