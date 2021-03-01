using System.Collections.Generic;

namespace TranslationCommon.Fonts
{
    public class UIFixesData
    {
        public Dictionary<string, UIActionFixes> ActionFixMap = new Dictionary<string, UIActionFixes>();

        /// <summary>
        ///     Get action fixes
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public UIActionFixes Get(string actionName)
        {
            if (ActionFixMap.ContainsKey(actionName))
            {
                return ActionFixMap[actionName];
            }

            return null;
        }
    }
}