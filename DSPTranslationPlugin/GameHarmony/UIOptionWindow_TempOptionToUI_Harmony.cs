using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DSPTranslationPlugin.UnityHarmony;
using HarmonyLib;
using TranslationCommon;
using TranslationCommon.Translation;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(UIOptionWindow), "TempOptionToUI")]
    public static class UIOptionWindow_TempOptionToUI_Harmony
    {
        private static int? _originalUICount;

        private static UIComboBox[] languageComboBoxes;

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

            if (languageComboBoxes == null)
                languageComboBoxes = new UIComboBox[InGameFonts.Length];

            // Needs to initialize UI
            if (_originalUICount == parent.childCount)
            {
                Localization.OnLanguageChange += ReloadFontsComboBox;
                
                for (int i = 0; i < InGameFonts.Length; i++)
                {
                    var fontName = InGameFonts[i];
                    // Add new combobox
                    var root = Object.Instantiate(genericComboBox,
                        genericComboBox.parent);

                    root.gameObject.SetActive(true);
                    Object.Destroy(root.GetComponent<Localizer>());
                    root.GetComponent<Text>().text = $"Font - {fontName}";

                    languageComboBoxes[i] = root.GetComponentInChildren<UIComboBox>();
                    UIComboBox languageComboBox = languageComboBoxes[i];
                    languageComboBox.Items.Clear();
                    languageComboBox.Items.Add(fontName);
                    foreach (var installedFont in TextFontManager.InstalledFonts)
                    {
                        languageComboBox.Items.Add(installedFont);
                    }


                    languageComboBox.text = fontName;

                    var settingsIndex = TextFontManager.GetSettingIndex(fontName);
                    ConsoleLogger.LogWarning("TextFontManager.GetSettingIndex " + settingsIndex);
                    languageComboBox.itemIndex = settingsIndex;

                    languageComboBox.onItemIndexChange.AddListener(() =>
                    {
                        var selectedFontName = languageComboBox.Items[languageComboBox.itemIndex];
                        if (selectedFontName == fontName)
                        {
                            TextFontManager.RestoreDefaultFont(fontName);
                        }
                        else
                        {
                            Font font;
                            try
                            {
                                if (TranslationManager.CurrentLanguage == null || TranslationManager.CurrentLanguage.Fonts == null)
                                    throw new InvalidOperationException();

                                font = TranslationManager.CurrentLanguage.Fonts.First(font1 => font1.name == selectedFontName);
                            }
                            catch (InvalidOperationException)
                            {
                                font = Font.CreateDynamicFontFromOSFont(selectedFontName, 12);
                            }

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

        public static void ReloadFontsComboBox(Language lang)
        {
            for (int index = 0; index < InGameFonts.Length; index++)
            {
                UIComboBox languageComboBox = languageComboBoxes[index];
                if (languageComboBox == null)
                {
                    return;
                }
                var fontName = InGameFonts[index];
                languageComboBox.Items.Clear();
                languageComboBox.Items.Add(fontName);
                foreach (var installedFont in TextFontManager.InstalledFonts)
                {
                    languageComboBox.Items.Add(installedFont);
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