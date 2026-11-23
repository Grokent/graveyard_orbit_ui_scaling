using GraveyardOrbit.UI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraveyardOrbit.UIScaling
{
    internal static class TopHudLayoutPatch
    {
        [HarmonyPatch(typeof(TopHudController), "Bind")]
        internal static class BindPatch
        {
            private static void Postfix(TopHudController __instance)
            {
                try
                {
                    VisualElement root = Traverse.Create(__instance)
                        .Field("_root")
                        .GetValue<VisualElement>();
                    if (root == null) return;

                    Label money = root.Q<Label>("money");
                    if (money != null)
                    {
                        // The wallet and the shard counters share the header
                        // row; centre the wallet text vertically like the
                        // counters instead of letting it ride the top edge.
                        money.style.unityTextAlign = TextAnchor.MiddleLeft;
                        money.style.paddingTop = 0f;
                        money.style.paddingBottom = 0f;
                    }

                    VisualElement meta = root.Q<VisualElement>("meta-wrap");
                    if (meta != null)
                    {
                        // A little breathing room after the last shard so the
                        // centred title does not touch the counters.
                        meta.style.paddingRight = 18f;
                        meta.style.marginRight = 6f;
                    }
                }
                catch
                {
                    // Keep the stock HUD if the private layout changes.
                }
            }
        }
    }
}
