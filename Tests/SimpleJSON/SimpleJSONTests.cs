using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using TranslationCommon.SimpleJSON;
using TranslationCommon.Translation;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            var a = new Dictionary<string, int>();
            var t = a.GetType();
        }
        
        [Test]
        public void LanguageDataTest()
        {
            LanguageData data = new LanguageData();
            data.TranslationTable = new List<TranslationProto>();
            var list = data.TranslationTable;
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TranslationProto($"Test {i}", i, $"Org {i}", $"Trans {i}"));
            }

            var truth = JsonConvert.SerializeObject(data, Formatting.Indented);
            NUnit.Framework.Assert.AreEqual(truth, JSON.ToJson(data, true));
            var test =  JSON.FromJson<LanguageData>(truth);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void ComplexDataTypesTest()
        {
            ComplexDataTypes data = ComplexDataTypes.GenerateRandomValues(null, false);

            var truth = JsonConvert.SerializeObject(data, Formatting.Indented);
            NUnit.Framework.Assert.AreEqual(truth, JSON.ToJson(data, true));
            var jsonNet = JsonConvert.DeserializeObject<ComplexDataTypes>(truth);
            var test =  JSON.FromJson<ComplexDataTypes>(truth);
            jsonNet.Should().BeEquivalentTo(data);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void ComplexDataTypesTestNoIndent()
        {
            ComplexDataTypes data = ComplexDataTypes.GenerateRandomValues(null, false);

            var truth = JsonConvert.SerializeObject(data, Formatting.None);
            NUnit.Framework.Assert.AreEqual(truth, JSON.ToJson(data, false));
            var test =  JSON.FromJson<ComplexDataTypes>(truth);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void UnityTypesTest()
        {
            UnityTypes data = UnityTypes.GenerateRandomValues();

            var result = JSON.ToJson(data, true);
            var test =  JSON.FromJson<UnityTypes>(result);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void DictionaryTest()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Key1", "Value1");
            data.Add("Key2", "Value2");
            
            var truth = JsonConvert.SerializeObject(data, Formatting.Indented);
            NUnit.Framework.Assert.AreEqual(truth, JSON.ToJson(data, true));
            var jsonNet = JsonConvert.DeserializeObject<Dictionary<string, string>>(truth);
            var test =  JSON.FromJson<Dictionary<string, string>>(truth);
            jsonNet.Should().BeEquivalentTo(data);
            test.Should().BeEquivalentTo(data);
        }

        [Test]
        public void SerializeFirstAsObjectTest()
        {
            var data = new SerializeFirstAsObjectTypes();
            data.Add("Test", "test");
            data.Add("Test2", "test2");
            
            var result = JSON.ToJson(data, true);
            NUnit.Framework.Assert.AreEqual("{\r\n  \"Test\": \"test\",\r\n  \"Test2\": \"test2\"\r\n}", result);
            var test = JSON.FromJson<SerializeFirstAsObjectTypes>(result);
            test.Get().Should().BeEquivalentTo(data.Get());
        }
    }
}