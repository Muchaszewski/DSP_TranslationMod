using System.Collections.Generic;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    /// <summary>
    ///     Cache of all possible fixes available to be used
    /// </summary>
    public class UIBehaviourCache
    {
        /// <summary>
        ///     List of fixes - serialized to json
        /// </summary>
        public UIFixesData UIFixes = new UIFixesData();

        /// <summary>
        ///     Get fixes for requested component
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public List<UIFix> GetFixes(string actionName, IBehaviourComponent component)
        {
            var fixes = new List<UIFix>();
            var action = UIFixes.Get(actionName);
            if (action == null)
            {
                return null;
            }

            fixes.AddRange(action.GetByType(component));
            fixes.AddRange(action.GetByPath(component));
            if (fixes.Count != 0)
            {
                ConsoleLogger.LogDebug($"Match! {fixes.Count} {component.GetComponentPath()}");
            }
            return fixes.Count != 0 ? fixes : null;
        }
    }
}