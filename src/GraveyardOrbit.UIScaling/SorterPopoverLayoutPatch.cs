using GraveyardOrbit.UI;
using HarmonyLib;
using UnityEngine.UIElements;

namespace GraveyardOrbit.UIScaling
{
    internal static class SorterPopoverLayoutPatch
    {
        [HarmonyPatch(typeof(RefineryScreen), nameof(RefineryScreen.OpenSocket))]
        internal static class OpenSocketPatch
        {
            private static void Postfix(RefineryScreen __instance, Machine m)
            {
                if (m == null || m.kind != MachineKind.Sorter) return;
                if (UiScale.Factor <= 1.01f) return;

                VisualElement pop = __instance.Popover;
                if (pop == null) return;

                VisualElement head = pop.Q<VisualElement>(null, "rl-pop__head");
                if (head == null) return;

                Label sub = head.Q<Label>(null, "rl-pop__sub");
                if (sub == null || sub.parent != head) return;

                int index = pop.IndexOf(head);
                sub.RemoveFromHierarchy();
                if (index >= 0) pop.Insert(index + 1, sub);
                else pop.Add(sub);

                sub.style.marginLeft = 0f;
                sub.style.marginTop = 4f;
                sub.style.marginBottom = 10f;
                sub.style.whiteSpace = WhiteSpace.Normal;
            }
        }
    }
}
