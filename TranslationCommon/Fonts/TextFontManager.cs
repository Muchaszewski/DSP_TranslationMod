using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DSPTranslationPlugin.UnityHarmony
{
    /// <summary>
    ///     Font manager for all text components
    /// </summary>
    public static class TextFontManager
    {
        /// <summary>
        ///     Original Font name and Custom font pair
        /// </summary>
        public static readonly Dictionary<string, CustomFontData> CustomFonts = new Dictionary<string, CustomFontData>();
        
        /// <summary>
        ///     All Text object references with it's corresponding font data
        /// </summary>
        private static readonly Dictionary<Text, TextDefaultFont> _textReferences = new Dictionary<Text, TextDefaultFont>();
        
        /// <summary>
        ///     Player prefs key for all saved fonts keys
        /// </summary>
        public const string PlayerPrefsSavedFontsCode = "dps_translate_muchaszewski_fonts";
        /// <summary>
        ///     Player prefs key for font used under given name
        /// </summary>
        public const string PlayerPrefsFontCode = "dps_translate_muchaszewski_font_";

        /// <summary>
        ///     List of all installed fonts
        /// </summary>
        public static string[] InstalledFonts { get; }
        
        /// <summary>
        ///     Static constructor
        /// </summary>
        static TextFontManager()
        {
            InstalledFonts = Font.GetOSInstalledFontNames();
            LoadSettings();
        }

        /// <summary>
        ///     Loads text font settings
        /// </summary>
        private static void LoadSettings()
        {
            var savedFonts = PlayerPrefs.GetString(PlayerPrefsSavedFontsCode);
            var split = savedFonts.Split('|').Where(s => !String.IsNullOrEmpty(s)).ToList();
            foreach (var savedFontType in split)
            {
                var fontName = PlayerPrefs.GetString(PlayerPrefsFontCode + savedFontType);
                if (fontName == savedFontType)
                {
                    RestoreDefaultFont(savedFontType);
                }
                else
                {
                    var font = Font.CreateDynamicFontFromOSFont(fontName, 12);
                    ApplyCustomFont(savedFontType, font);
                }
            }
        }

        /// <summary>
        ///     Get all currently existing text elements
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TextDefaultFont> GetExistingTextsElements()
        {
            return _textReferences.Values;
        }
        
        /// <summary>
        ///     Get text data by it's reference
        /// </summary>
        /// <param name="text">Text reference</param>
        /// <returns></returns>
        public static TextDefaultFont Get(Text text)
        {
            _textReferences.TryGetValue(text, out var value);
            return value;
        }

        /// <summary>
        ///     Add text data by it's reference
        /// </summary>
        /// <param name="text">Text reference</param>
        public static void Add(Text text)
        {
            var find = Get(text);
            if (find == null)
            {
                _textReferences.Add(text, new TextDefaultFont(text));
            }
        }
        
        /// <summary>
        ///     Remove text data by it's reference
        /// </summary>
        /// <param name="text">Text reference</param>
        public static void Remove(Text text)
        {
            var find = Get(text);
            if (find != null)
            {
                _textReferences.Remove(text);
            }        
        }
        
        /// <summary>
        ///     Get setting index for menu settings
        /// </summary>
        /// <param name="key">Font key</param>
        /// <returns></returns>
        public static int GetSettingIndex(string key)
        {
            if (CustomFonts.ContainsKey(key))
            {
                if (!CustomFonts[key].IsUsingCustomFont) return 0;
                var fontName = CustomFonts[key].CustomFont.name;
                for (int i = 0; i < InstalledFonts.Length; i++)
                {
                    if(InstalledFonts[i] != fontName) continue;
                    return i + 1;
                }
            }

            return 0;
        }
        
        /// <summary>
        ///     Saves settings into player prefs
        /// </summary>
        /// <param name="key">Font key</param>
        /// <returns></returns>
        public static void SaveSettings(string key, string fontName)
        {
            var savedFonts = PlayerPrefs.GetString(PlayerPrefsSavedFontsCode);
            var split = savedFonts.Split('|').Where(s => !String.IsNullOrEmpty(s)).ToList();
            if (!split.Contains(key))
            {
                split.Add(key);
            }
            PlayerPrefs.SetString(PlayerPrefsFontCode + key, fontName);
            PlayerPrefs.SetString(PlayerPrefsSavedFontsCode, String.Join("|", split.ToArray()));
        }

        /// <summary>
        ///     Apply provided font to font key group
        /// </summary>
        /// <param name="key">Font key</param>
        /// <param name="fontToUse">Font to apply</param>
        public static void ApplyCustomFont(string key, Font fontToUse)
        {
            CustomFonts[key] = new CustomFontData()
            {
                CustomFont = fontToUse,
                IsUsingCustomFont = true,
            };
            SaveSettings(key, fontToUse.name);
            foreach (var text in _textReferences)
            {
                text.Value.UseCustomFontImmediate(fontToUse);
            }
        }
        
        /// <summary>
        ///     Restore default font
        /// </summary>
        /// <param name="key">Font key</param>
        public static void RestoreDefaultFont(string key)
        {
            CustomFonts[key] = new CustomFontData()
            {
                IsUsingCustomFont = false,
            };
            SaveSettings(key, key);
            foreach (var text in _textReferences)
            {
                text.Value.UseCustomFontImmediate(text.Value.DefaultFont);
            }
        }
    }
}