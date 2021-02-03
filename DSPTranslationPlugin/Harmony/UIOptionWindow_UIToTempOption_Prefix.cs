using System.Reflection;
using HarmonyLib;
using TranslationCommon;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(UIOptionWindow), "UIToTempOption")]
    public static class UIOptionWindow_UIToTempOption_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix(UIOptionWindow __instance)
        {
            UIComboBox comboBox = (UIComboBox)__instance.GetType().GetField("languageComp", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(__instance);
            var item = comboBox.Items[comboBox.itemIndex];
            TranslationManager.SelectedLanguage = item;
        }
    }
}