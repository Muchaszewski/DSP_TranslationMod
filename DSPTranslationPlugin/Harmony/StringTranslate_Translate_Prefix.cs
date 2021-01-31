using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TranslationCommon;
using UnityEngine;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(StringTranslate), nameof(StringTranslate.Translate), typeof(string))]
    public static class StringTranslate_Translate_Prefix
    {
        [HarmonyPrefix]
        public static bool Prefix(ref string __result, string s)
        {
            if (s == null)
            {
                return true;
            }

            if (TranslationManager.CurrentLanguage != null)
            {
                if (TranslationManager.TranslationDictionary.ContainsKey(s))
                {
                    __result = TranslationManager.TranslationDictionary[s].Translation;
                    return false;
                }

                return true;
            }
            
            return true;
        }
    }
    
    [HarmonyPatch(typeof(GlobalObject), nameof(GlobalObject.SaveLastLanguage))]
    public static class GlobalObject_SaveLastLanguage_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!TranslationManager.IsInitialized)
            {
                TranslationManager.LoadCurrentLanguage();
            }
            else
            {
                if (TranslationManager.CurrentLanguage == null)
                {
                    PlayerPrefs.DeleteKey(TranslationManager.PlayerPrefsCode);
                }
                else
                {
                    PlayerPrefs.SetString(TranslationManager.PlayerPrefsCode, TranslationManager.CurrentLanguage.Settings.LanguageDisplayName);
                }
                PlayerPrefs.Save();
            }
        }
    }
    
    [HarmonyPatch(typeof(GlobalObject), "Initialize")]
    public static class GlobalObject_Initialize_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            var result = PlayerPrefs.GetString(TranslationManager.PlayerPrefsCode);
            if (!String.IsNullOrEmpty(result))
            {
                TranslationManager.SelectedLanguage = result;
                TranslationManager.LoadCurrentLanguage();
            }
        }
        
    }

    [HarmonyPatch(typeof(GameOption), nameof(GameOption.Apply))]
    public static class GameOption_Apply_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            TranslationManager.LoadCurrentLanguage();
        }
    }
    
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
    }
}