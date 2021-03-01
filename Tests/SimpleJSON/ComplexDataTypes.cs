using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
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