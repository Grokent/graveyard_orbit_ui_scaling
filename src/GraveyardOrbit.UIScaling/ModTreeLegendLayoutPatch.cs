using GraveyardOrbit.UI;
using HarmonyLib;
using UnityEngine.UIElements;

namespace GraveyardOrbit.UIScaling
{
    /// <summary>
    /// The NODE KEY legend is a card with room to grow. Let it fill the info
    /// column and wrap its rows rather than clipping the scaled text.
    /// </summary>
    internal static class ModTreeLegendLayoutPatch
    {
        [HarmonyPatch(typeof(ModTreeScreen), "MakeLegend")]
        internal static class MakeLegendPatch
        {
            private static void Postfix(VisualElement __result)
            {
                if (__result == null || UiScale.Factor <= 1.01f) return;

                __result.style.alignSelf = Align.Stretch;
                __result.style.width = Length.Percent(100f);

                foreach (VisualElement row in __result.Children())
                {
                    if (row is Label) continue;

                    row.style.width = Length.Percent(100f);
                    foreach (VisualElement child in row.Children())
                    {
                        if (child is not Label label) continue;
                        label.style.whiteSpace = WhiteSpace.Normal;
                        label.style.flexShrink = 1f;
                        label.style.minWidth = 0f;
                        label.style.paddingRight = 0f;
                    }
                }
            }
        }
    }
}
