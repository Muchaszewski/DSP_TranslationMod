using System.Collections.Generic;
using System.Reflection;
using DSPTranslationPlugin.UnityHarmony;
using HarmonyLib;
using TranslationCommon;
using TranslationCommon.Translation;
using UnityEngine;
using UnityEngine.UI;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(UIOptionWindow), "TempOptionToUI")]
    public static class UIOptionWindow_TempOptionToUI_Harmony
    {
        private static int? _originalUICount;

        private static string[] InGameFonts = new[]
        {
            "Default",
        };
        
        [HarmonyPrefix]
        public static void Prefix(UIOptionWindow __instance)
        {
            AddComboBoxLanguage(__instance);
        }
        
        [HarmonyPostfix]
        public static void Postfix(UIOptionWindow __instance)
        {
            ApplyComboBoxLanguage(__instance);
            CreateFontCompoBox(__instance);
        }

        /// <summary>
        ///     Create option combobox with selection of font
        /// </summary>
        private static void CreateFontCompoBox(UIOptionWindow __instance)
        {
            var parent = __instance.languageComp.transform.parent.parent;
            if (_originalUICount == null)
            {
                _originalUICount = parent.childCount;
            }

            var genericComboBox = __instance.tipLevelComp.transform.parent;

            // Needs to initialize UI
            if (_originalUICount == parent.childCount)
            {
                foreach (var fontName in InGameFonts)
                {
                    // Add new combobox
                    var root = Object.Instantiate(genericComboBox,
                        genericComboBox.parent);

                    root.gameObject.SetActive(true);
                    Object.Destroy(root.GetComponent<Localizer>());
                    root.GetComponent<Text>().text = $"Font - {fontName}";

                    var newComboBox = root.GetComponentInChildren<UIComboBox>();
                    newComboBox.Items.Clear();
                    newComboBox.Items.Add(fontName);
                    foreach (var installedFont in TextFontManager.InstalledFonts)
                    {
                        newComboBox.Items.Add(installedFont);
                    }

                    newComboBox.text = fontName;

                    var settingsIndex = TextFontManager.GetSettingIndex(fontName);
                    ConsoleLogger.LogWarning("TextFontManager.GetSettingIndex " + settingsIndex);
                    newComboBox.itemIndex = settingsIndex;

                    newComboBox.onItemIndexChange.AddListener(() =>
                    {
                        var selectedFontName = newComboBox.Items[newComboBox.itemIndex];
                        if (selectedFontName == fontName)
                        {
                            TextFontManager.RestoreDefaultFont(fontName);
                        }
                        else
                        {
                            var font = Font.CreateDynamicFontFromOSFont(selectedFontName, 12);
                            TextFontManager.ApplyCustomFont(fontName, font);
                        }
                    });

                    // Set Option position
                    var rectTransform = root.GetComponent<RectTransform>();
                    var childCountWithoutRestore = __instance.languageComp.transform.parent.parent.childCount - 2;
                    var position = __instance.languageComp.transform.parent.GetComponent<RectTransform>().anchoredPosition;
                    var offset = 40;
                    rectTransform.anchoredPosition = new Vector2(position.x, position.y - offset * childCountWithoutRestore);
                }
            }
        }

        private static void AddComboBoxLanguage(UIOptionWindow __instance)
        {
            if (!__instance.languageComp.Items.Contains("(Original) Française"))
            {
                __instance.languageComp.Items.Add("(Original) Française");
            }

            foreach (var langauge in TranslationManager.Langauges)
            {
                if (!__instance.languageComp.Items.Contains(langauge.Settings.LanguageDisplayName))
                {
                    __instance.languageComp.Items.Add(langauge.Settings.LanguageDisplayName);
                }
            }
        }

        private static void ApplyComboBoxLanguage(UIOptionWindow __instance)
        {
            if (TranslationManager.CurrentLanguage != null)
            {
                for (int i = 0; i < TranslationManager.Langauges.Count; i++)
                {
                    if (TranslationManager.CurrentLanguage.Settings.LanguageDisplayName ==
                        TranslationManager.Langauges[i].Settings.LanguageDisplayName)
                        __instance.languageComp.itemIndex = 3 + i;
                }
            }
        }
    }
}