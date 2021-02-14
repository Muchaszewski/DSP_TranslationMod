using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using TranslationCommon;
using TranslationCommon.Translation;

namespace SimpleJSONTests
{
    public class ReflectionTests
    {
        public class StringProto
        {
            public string ZHCN;
            public string ENUS;
            public string FRFR;
        }
        
        [Test]
        public void GetTextFromStringProtoTest()
        {
            var settings = new LanguageSettings();
            settings.OriginalLanguage = "ENUS";
            var val = new StringProto()
            {
                ENUS = "ENUS",
                FRFR = "FRFR",
                ZHCN = "ZHCN",
            };

            var translationDelegate = LanguageData.GetOriginalTextDelegate<StringProto>(settings);
            Assert.AreEqual("ENUS", translationDelegate(val));
        }
    }
}