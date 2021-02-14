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
        private static UIComboBox _languageComboBox;
        private static UIComboBox _tipLevelComboBox;

        private static string[] InGameFonts = new[]
        {
            "Default",
        };
        
        [HarmonyPrefix]
        public static void Prefix(UIOptionWindow __instance)
        {
            if (_languageComboBox == null)
            {
                // ReSharper disable PossibleNullReferenceException
                _languageComboBox = (UIComboBox) __instance.GetType()
                    .GetField("languageComp", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(__instance);
                _tipLevelComboBox = (UIComboBox) __instance.GetType()
                    .GetField("tipLevelComp", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(__instance);
                // ReSharper restore PossibleNullReferenceException
            }
            
            AddComboBoxLanguage(__instance);
        }
        
        [HarmonyPostfix]
        public static void Postfix(UIOptionWindow __instance)
        {
            ApplyComboBoxLanguage(__instance);
            CreateFontCompoBox();
        }

        /// <summary>
        ///     Create option combobox with selection of font
        /// </summary>
        private static void CreateFontCompoBox()
        {
            var parent = _languageComboBox.transform.parent.parent;
            if (_originalUICount == null)
            {
                _originalUICount = parent.childCount;
            }

            var genericComboBox = _tipLevelComboBox.transform.parent;

            // Needs to initialize UI
            if (_originalUICount == parent.childCount)
            {
                var prevPosition = genericComboBox.GetComponent<RectTransform>();

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
                    rectTransform.anchoredPosition =
                        prevPosition.anchoredPosition - new Vector2(0, prevPosition.sizeDelta.y + 5);
                    prevPosition = rectTransform;
                }
            }
        }

        private static void AddComboBoxLanguage(UIOptionWindow __instance)
        {
            if (!_languageComboBox.Items.Contains("(Original) Française"))
            {
                _languageComboBox.Items.Add("(Original) Française");
            }

            foreach (var langauge in TranslationManager.Langauges)
            {
                if (!_languageComboBox.Items.Contains(langauge.Settings.LanguageDisplayName))
                {
                    _languageComboBox.Items.Add(langauge.Settings.LanguageDisplayName);
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
                        _languageComboBox.itemIndex = 3 + i;
                }
            }
        }
    }
}