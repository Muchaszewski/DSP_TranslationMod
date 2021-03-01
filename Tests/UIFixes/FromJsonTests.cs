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
            Assert.IsTrue(data.ActionFixMap.Count == 1);
            var action = data.ActionFixMap["OnInit"];
            Assert.NotNull(action);
            var fixes = action.GetByType(new TestableUIBehaviourComponent()
            {
                Type = "UIOptionWindow"
            });
            Assert.AreEqual(1, fixes.Count);
            Assert.AreEqual("*details.content*.*", fixes[0].Path);
            Assert.IsInstanceOf<RectFix>(fixes[0].Fix);
            var rectFix = (RectFix)fixes[0].Fix;
            Assert.AreEqual("(+75, +0)", rectFix.anchoredPosition);
        }
    }
}