using System.Reflection;
using HarmonyLib;
using TranslationCommon.Translation;
using UnityEngine;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(UIOptionWindow), "UIToTempOption")]
    public static class UIOptionWindow_UIToTempOption_Harmony
    {
        [HarmonyPrefix]
        public static void Prefix(UIOptionWindow __instance)
        {
            UIComboBox comboBox = __instance.languageComp;
            var item = comboBox.itemIndex != -1
                ? comboBox.Items[comboBox.itemIndex]
                : "English";
            TranslationManager.SelectedLanguage = item;
        }
    }
}