using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DSPTranslationPlugin.GameHarmony
{
    /// <summary>
    ///     Localizer postfix for expanding translation box to match new credits text
    /// </summary>
    [HarmonyPatch(typeof(Localizer), "Refresh")]
    public static class Localizer_Refresh_Harmony
    {
        [HarmonyPrefix]
        public static bool Prefix(Localizer __instance)
        {
            var maskableGraphics = __instance.GetComponents<MaskableGraphic>();
            __instance.translation = __instance.stringKey.Translate();
            foreach (var graphic in maskableGraphics)
            {
                if (graphic is Text text)
                {
                    text.text = __instance.translation;
                }
                else if (graphic is Image image)
                {
                    if (Uri.IsWellFormedUriString(__instance.translation, UriKind.RelativeOrAbsolute))
                    {
                        __instance.StartCoroutine(GetRequest(__instance.translation, image));
                    }
                    else
                    {
                        image.sprite = Resources.Load<Sprite>(__instance.translation);
                    }
                }
                else if (graphic is RawImage rawImage)
                {
                    if (Uri.IsWellFormedUriString(__instance.translation, UriKind.RelativeOrAbsolute))
                    {
                        __instance.StartCoroutine(GetRequest(__instance.translation, rawImage));
                    }
                    else
                    {
                        rawImage.texture = Resources.Load<Texture2D>(__instance.translation);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Expand in game credits box
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        public static void Postfix(Localizer __instance)
        {
            ExpandGameCreditsBox(__instance);
        }

        private static void ExpandGameCreditsBox(Localizer __instance)
        {
            if (__instance.name == "tip" && __instance.transform.parent.name == "language")
            {
                var rect = __instance.GetComponent<RectTransform>();
                var sizeDelta = rect.sizeDelta;
                sizeDelta.x = 600;
                sizeDelta.y = 90;
                rect.sizeDelta = sizeDelta;
            }
        }

        private static IEnumerator GetRequest(string uri, RawImage image)
        {
            var www = UnityWebRequestTexture.GetTexture(uri);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
                image.texture = myTexture;
            }
        }

        private static IEnumerator GetRequest(string uri, Image image)
        {
            var www = UnityWebRequestTexture.GetTexture(uri);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
                var sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), Vector2.zero);
                image.sprite = sprite;
            }
        }
    }
}