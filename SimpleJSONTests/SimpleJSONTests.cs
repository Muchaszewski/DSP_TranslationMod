using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using TranslationCommon;
using UnityEngine;
using Random = System.Random;

namespace SimpleJSONTests
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
            Assert.AreEqual(truth, SimpleJSON.JSON.ToJson(data, true));
            var test =  SimpleJSON.JSON.FromJson<LanguageData>(truth);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void ComplexDataTypesTest()
        {
            ComplexDataTypes data = ComplexDataTypes.GenerateRandomValues(null, false);

            var truth = JsonConvert.SerializeObject(data, Formatting.Indented);
            Assert.AreEqual(truth, SimpleJSON.JSON.ToJson(data, true));
            var jsonNet = JsonConvert.DeserializeObject<ComplexDataTypes>(truth);
            var test =  SimpleJSON.JSON.FromJson<ComplexDataTypes>(truth);
            jsonNet.Should().BeEquivalentTo(data);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void ComplexDataTypesTestNoIndent()
        {
            ComplexDataTypes data = ComplexDataTypes.GenerateRandomValues(null, false);

            var truth = JsonConvert.SerializeObject(data, Formatting.None);
            Assert.AreEqual(truth, SimpleJSON.JSON.ToJson(data, false));
            var test =  SimpleJSON.JSON.FromJson<ComplexDataTypes>(truth);
            test.Should().BeEquivalentTo(data);
        }
        
        [Test]
        public void UnityTypesTest()
        {
            UnityTypes data = UnityTypes.GenerateRandomValues();

            var result = SimpleJSON.JSON.ToJson(data, true);
            var test =  SimpleJSON.JSON.FromJson<UnityTypes>(result);
            test.Should().BeEquivalentTo(data);
        }
    }

    public class UnityTypes
    {
        public Vector2 Vector2Field;
        public Vector3 Vector3Field;
        
        public List<Vector2> ListVector2Field;

        public static UnityTypes GenerateRandomValues()
        {
            UnityTypes value = new UnityTypes();
            var random = new Random(0);
            value.Vector2Field = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            value.Vector3Field = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            
            value.ListVector2Field = new List<Vector2>();
            for (int i = 0; i < 3; i++)
            {
                value.ListVector2Field.Add(new Vector2((float)random.NextDouble(), (float)random.NextDouble()));
            }
            
            return value;
        }
    }
    
    public class ComplexDataTypes
    {
        public bool BoolField;
        public byte ByteField;
        public short ShortField;
        public ushort UshortField;
        public int IntField;
        public uint UintField;
        public char CharField;
        public float FloatField;
        public double DoubleField;
        public long LongField;
        public ulong UlongField;
        public decimal DecimalField;
        public sbyte SbyteField;

        public string StringField;

        public object UnknownTypeField;
        public ComplexDataTypes ParentField;
        public ComplexDataTypes ChildField;
        
        public List<ComplexDataTypes> ListTypeField;
        public List<string> ListStringField;
        public List<int> ListIntField;

        public Dictionary<string, string> StringStringDictionaryField;
        public Dictionary<int, int> IntIntDictionaryField;
        public Dictionary<ComplexDataTypes, ComplexDataTypes> ComplexComplexDictionaryField;

        public static ComplexDataTypes GenerateRandomValues(ComplexDataTypes parent = null, bool generateLoop = true)
        {
            ComplexDataTypes value = new ComplexDataTypes();
            var random = new Random(0);
            value.BoolField = random.NextDouble() > 0.5 ? true : false;
            value.ByteField = (byte) random.Next();
            value.CharField = (char) random.Next();
            value.ShortField = (short) random.Next();
            value.UshortField = (ushort) random.Next();
            value.IntField = random.Next();
            value.UintField = (uint) random.Next();
            value.FloatField = (float) random.NextDouble();
            value.DoubleField = random.NextDouble();
            value.LongField = (long) random.Next();
            value.UlongField = (ulong) random.Next();
            value.DecimalField = (decimal) random.NextDouble();
            value.SbyteField = (sbyte) random.Next();

            value.StringField = RandomStringGenerator.RandomString(8);
            
            value.UnknownTypeField = new object();

            if (generateLoop)
            {
                value.ParentField = parent;
            }
            value.ChildField = parent == null ? GenerateRandomValues(value, generateLoop) : null;

            if (parent == null)
            {
                value.ListTypeField = new List<ComplexDataTypes>();
                for (int i = 0; i < 3; i++)
                {
                    value.ListTypeField.Add(GenerateRandomValues(value, generateLoop));
                }
            }
            value.ListStringField = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                value.ListStringField.Add(RandomStringGenerator.RandomString(8));
            }
            value.ListIntField = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                value.ListIntField.Add(random.Next());
            }

            return value;
        }
        
        internal static class RandomStringGenerator
        {
            /// <summary>
            /// Generate and return a random number 
            /// </summary>
            /// <returns>random number</returns>
            public static int RandomNumber()
            {
                Random random = new Random();
                return random.Next(1000, 5000);
            }

            /// <summary>
            /// Generate and return a random string    
            /// </summary>
            /// <param name="length">length of the string</param>
            /// <returns>random string</returns>
            public static string RandomString(int length)
            {
                StringBuilder strbuilder = new StringBuilder();
                Random random = new Random();
                for (int i = 0; i < length; i++)
                {
                    // Generate floating point numbers
                    double myFloat = random.NextDouble();
                    // Generate the char
                    var myChar = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * myFloat) + 65));
                    strbuilder.Append(myChar);
                }
                return strbuilder.ToString().ToLower();
            }      
        }
    }
}