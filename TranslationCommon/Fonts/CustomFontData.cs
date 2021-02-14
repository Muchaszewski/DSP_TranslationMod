using Font = UnityEngine.Font;

namespace DSPTranslationPlugin.UnityHarmony
{
    /// <summary>
    ///     Custom font data container
    /// </summary>
    public class CustomFontData
    {
        /// <summary>
        ///     Is using default font or custom font
        /// </summary>
        public bool IsUsingCustomFont;
        /// <summary>
        ///     Custom font data
        /// </summary>
        public Font CustomFont;
    }
}