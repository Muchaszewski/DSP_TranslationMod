using System;
using UnityEngine;
using UnityEngine.UI;

namespace TranslationCommon.Fonts
{
    public class TextFix : IFix
    {
        public TextAnchor? alignment;
        public string fontSize;
        public FontStyle? fontStyle;
        public HorizontalWrapMode? horizontalOverflow;
        public string lineSpacing;
        public VerticalWrapMode? verticalOverflow;
        public bool? alignByGeometry;
        public bool? resizeTextForBestFit;
        public string resizeTextMaxSize;
        public string resizeTextMinSize;
        public Color? color;

        
        public void Evaluate(UIFix parent, IBehaviourComponent component)
        {
            var texts = parent.GetComponents<Text>(component);
            foreach (var text in texts)
            {
                Fix(text);
            }
        }

        private void Fix(Text text)
        {
            if (alignment != null)
            {
                text.alignment = alignment.Value;
            }
            if (!string.IsNullOrEmpty(fontSize))
            {
                text.fontSize = text.fontSize.SetRelative(fontSize);
            }
            if (fontStyle != null)
            {
                text.fontStyle = fontStyle.Value;
            }
            if (horizontalOverflow != null)
            {
                text.horizontalOverflow = horizontalOverflow.Value;
            }
            if (!string.IsNullOrEmpty(lineSpacing))
            {
                text.lineSpacing = text.lineSpacing.SetRelative(lineSpacing);
            }
            if (verticalOverflow != null)
            {
                text.verticalOverflow = verticalOverflow.Value;
            }
            if (alignByGeometry != null)
            {
                text.alignByGeometry = alignByGeometry.Value;
            }
            if (resizeTextForBestFit != null)
            {
                text.resizeTextForBestFit = resizeTextForBestFit.Value;
            }
            if (!string.IsNullOrEmpty(resizeTextMaxSize))
            {
                text.resizeTextMaxSize = text.resizeTextMaxSize.SetRelative(resizeTextMaxSize);
            }
            if (!string.IsNullOrEmpty(resizeTextMinSize))
            {
                text.resizeTextMinSize = text.resizeTextMinSize.SetRelative(resizeTextMinSize);
            }
            if (color != null)
            {
                text.color = color.Value;
            }
        }
    }
}