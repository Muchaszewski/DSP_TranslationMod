using UnityEngine;

namespace TranslationCommon.Fonts
{
    public class RectFix : IFix
    {
        public string pivot;
        public string anchoredPosition;
        public string anchorMax;
        public string anchorMin;
        public string offsetMax;
        public string offsetMin;
        public string sizeDelta;

        public void Evaluate(UIFix parent, IBehaviourComponent component)
        {
            var rectTransforms = parent.GetComponents<RectTransform>(component);
            foreach (var transform in rectTransforms)
            {
                Fix(transform);
            }
        }
        
        private void Fix(RectTransform rTrans)
        {
            if (!string.IsNullOrEmpty(pivot))
            {
                rTrans.pivot = rTrans.pivot.SetRelative(pivot);
            }
            if (!string.IsNullOrEmpty(anchoredPosition))
            {
                rTrans.anchoredPosition = rTrans.anchoredPosition.SetRelative(anchoredPosition);
            }
            if (!string.IsNullOrEmpty(anchorMax))
            {
                rTrans.anchorMax = rTrans.anchorMax.SetRelative(anchorMax);
            }
            if (!string.IsNullOrEmpty(anchorMin))
            {
                rTrans.anchorMin = rTrans.anchorMin.SetRelative(anchorMin);
            }
            if (!string.IsNullOrEmpty(offsetMax))
            {
                rTrans.offsetMax = rTrans.offsetMax.SetRelative(offsetMax);
            }
            if (!string.IsNullOrEmpty(offsetMin))
            {
                rTrans.offsetMin = rTrans.offsetMin.SetRelative(offsetMin);
            }
            if (!string.IsNullOrEmpty(sizeDelta))
            {
                rTrans.sizeDelta = rTrans.sizeDelta.SetRelative(sizeDelta);
            }
        }
    }
}