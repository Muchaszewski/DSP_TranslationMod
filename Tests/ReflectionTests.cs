using NUnit.Framework;
using TranslationCommon.Translation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Tests
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
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("ENUS", translationDelegate(val));
        }
    }
}