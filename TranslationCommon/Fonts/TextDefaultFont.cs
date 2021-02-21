using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DSPTranslationPlugin.UnityHarmony
{
    /// <summary>
    ///     Additional container for Text component - this should be always present but it's not a monobehaviour.
    /// <para>Use static api of <see cref="TextFontManager.Get"/> to request underlying font information</para>
    /// </summary>
    public class TextDefaultFont
    {
        /// <summary>
        ///     Field info helper to access font data
        /// </summary>
        private readonly static FieldInfo FieldInfo_FontData = AccessTools.Field(typeof(Text), "m_FontData");

        /// <summary>
        ///     Default font used by this component
        /// </summary>
        public Font DefaultFont;
        
        /// <summary>
        ///     Stored text refernece
        /// </summary>
        public Text Reference;
        
        /// <summary>
        ///     FontData private field reference
        /// </summary>
        private FontData FontData;

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="reference"></param>
        public TextDefaultFont(Text reference)
        {
            Reference = reference;
            FontData = (FontData)FieldInfo_FontData.GetValue(Reference);
            DefaultFont = FontData.font;
        }

        /// <summary>
        ///     Method invoked exclusively by <see cref="Text_Font_Gettter_Harmony"/>
        /// </summary>
        public void OnGetFont()
        {
            foreach (var customFont in TextFontManager.CustomFonts)
            {
                if (customFont.Key == DefaultFont.name || 
                    (customFont.Key == "Default" && !TextFontManager.CustomFonts.ContainsKey(DefaultFont.name)))
                {
                    if (customFont.Value.IsUsingCustomFont)
                    {
                        if (FontData.font != customFont.Value.CustomFont)
                        {
                            FontData.font = customFont.Value.CustomFont;
                        }
                    }
                    else
                    {
                        if (FontData.font != DefaultFont)
                        {
                            FontData.font = DefaultFont;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Apply custom fond immediately skipping TextFontManager
        /// </summary>
        /// <param name="fontToUse"></param>
        public void UseCustomFontImmediate(Font fontToUse)
        {
            Reference.font = fontToUse;
        }
    }
}