using System.Linq;
using NUnit.Framework;
using TranslationCommon.Fonts;
using TranslationCommon.SimpleJSON;

namespace Tests.UIFixes
{
    [TestFixture]
    public class FromJsonTests
    {
        [Test]
        public void TestFromJsonBasic()
        {
            string json =
                "{\"ActionFixMap\":{\"OnInit\":{\"t:UIOptionWindow\":[{\"Path\":\"*details.content*.*\",\"Fix\":{\"anchoredPosition\":\"(+75, +0)\"}}]}}}";
            var data = JSON.FromJson<UIFixesData>(json);
            NUnit.Framework.Assert.IsTrue(data.ActionFixMap.Count == 1);
            var action = data.ActionFixMap["OnInit"];
            Assert.NotNull(action);
            var fixes = action.GetByType(new TestableUIBehaviourComponent()
            {
                Type = "UIOptionWindow"
            });
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("*details.content*.*", fixes[0].Path);
            NUnit.Framework.Assert.IsInstanceOf<RectFix>(fixes[0].Fix[0]);
            var rectFix = (RectFix)fixes[0].Fix[0];
            NUnit.Framework.Assert.AreEqual("(+75, +0)", rectFix.anchoredPosition);
        }
    }
}