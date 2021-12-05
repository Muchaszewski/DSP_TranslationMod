using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TranslationCommon.Fonts;
using TranslationCommon.SimpleJSON;

namespace Tests.UIFixes
{
    [TestFixture]
    public class SimpleUIFixesTests
    {
        public UIBehaviourCache Cache = new UIBehaviourCache();
        public List<TestableUIBehaviourComponent> Components = new List<TestableUIBehaviourComponent>();

        [SetUp]
        public void Setup()
        {
            Cache = new UIBehaviourCache();
            var actionFixesMap = Cache.UIFixes.ActionFixMap;
            var actionFixes = new UIActionFixes();
            actionFixesMap.Add("OnCreate", actionFixes);
            var dictionaryFixes = new Dictionary<string, List<UIFix>>();
            dictionaryFixes.Add("t:UIAction", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "TypeTest"
                }
            });
            dictionaryFixes.Add("p:simplePath", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "SimplePath"
                }
            });
            dictionaryFixes.Add("p:*wildCardPath", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "StartWildCardPath"
                }
            });
            dictionaryFixes.Add("p:wildCardPath*", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "EndWildCardPath"
                }
            });
            dictionaryFixes.Add("p:*multiMatches*", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "MultiWildCardPath"
                }
            });
            dictionaryFixes.Add("p:*details.content*.*", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "ComplexWildCardPath"
                }
            });
            actionFixes.Initialize(dictionaryFixes);
            
            var actionFixes2 = new UIActionFixes();
            actionFixesMap.Add("OnInit", actionFixes2);
            var dictionaryFixes2 = new Dictionary<string, List<UIFix>>();
            dictionaryFixes2.Add("t:UIAction", new List<UIFix>()
            {
                new UIFix()
                {
                    Path = "TypeTest"
                }
            });
            actionFixes2.Initialize(dictionaryFixes2);
            CreateBehaviourComponents();
        }

        private void CreateBehaviourComponents()
        {
            Components.Add(new TestableUIBehaviourComponent()
            {
                Path = "Path",
                Type = "UIAction",
            });
        }
        
        [Test]
        public void TestableUIBehaviour_TypeTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "Path",
                Type = "UIAction",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("TypeTest", fixes[0].Path);
        }
        
        [Test]
        public void TestableUIBehaviour_PathTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "simplePath",
                Type = "Type",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("SimplePath", fixes[0].Path);
        }
        
        [Test]
        public void TestableUIBehaviour_StartWildCardPathTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "this.is.some.wildCardPath",
                Type = "Type",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("StartWildCardPath", fixes[0].Path);
        }
        
        [Test]
        public void TestableUIBehaviour_EndWildCardPathTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "wildCardPath.start.with.explosion!",
                Type = "Type",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("EndWildCardPath", fixes[0].Path);
        }
        
        [Test]
        public void TestableUIBehaviour_MultiWildCardPathTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "wildcards.multiMatches.rocks",
                Type = "Type",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("MultiWildCardPath", fixes[0].Path);
            testable = new TestableUIBehaviourComponent()
            {
                Path = "multiMatches.rocks",
                Type = "Type",
            };
            fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("MultiWildCardPath", fixes[0].Path);
            testable = new TestableUIBehaviourComponent()
            {
                Path = "wildcards.multiMatches",
                Type = "Type",
            };
            fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("MultiWildCardPath", fixes[0].Path);
        }
        
        [Test]
        public void TestableUIBehaviour_MultiMatches()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "simplePath",
                Type = "UIAction",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(2, fixes.Count);
            NUnit.Framework.Assert.IsTrue(fixes.Select(x => x.Path).Contains("SimplePath"));
            NUnit.Framework.Assert.IsTrue(fixes.Select(x => x.Path).Contains("TypeTest"));
        }
        
        [Test]
        public void TestableUIBehaviour_ComplexWildCardPathTest()
        {
            var testable = new TestableUIBehaviourComponent()
            {
                Path = "Root.Menu.UI.details.content-1.path",
                Type = "Type",
            };
            var fixes = Cache.GetFixes("OnCreate", testable);
            NUnit.Framework.Assert.AreEqual(1, fixes.Count);
            NUnit.Framework.Assert.AreEqual("ComplexWildCardPath", fixes[0].Path);
        }
    }
}