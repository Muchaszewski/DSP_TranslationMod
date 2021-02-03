using System.Reflection;
using HarmonyLib;
using TranslationCommon;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(UIOptionWindow), "TempOptionToUI")]
    public static class UIOptionWindow_TempOptionToUI_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix(UIOptionWindow __instance)
        {
            UIComboBox comboBox = (UIComboBox)__instance.GetType().GetField("languageComp", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(__instance);

            if (!comboBox.Items.Contains("(Original) Française"))
            {
                comboBox.Items.Add("(Original) Française");
            }
            foreach (var langauge in TranslationManager.Langauges)
            {
                if (!comboBox.Items.Contains(langauge.Settings.LanguageDisplayName))
                {
                    comboBox.Items.Add(langauge.Settings.LanguageDisplayName);
                }
            }
        }

        [HarmonyPostfix]
        public static void Postfix(UIOptionWindow __instance)
        {            
            UIComboBox comboBox = (UIComboBox)__instance.GetType().GetField("languageComp", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(__instance);
            if (TranslationManager.CurrentLanguage != null)
            {
                for (int i = 0; i < TranslationManager.Langauges.Count; i++)
                {
                    if (TranslationManager.CurrentLanguage.Settings.LanguageDisplayName ==
                        TranslationManager.Langauges[i].Settings.LanguageDisplayName)
                        comboBox.itemIndex = 3 + i;
                }
            }
        }
    }
}