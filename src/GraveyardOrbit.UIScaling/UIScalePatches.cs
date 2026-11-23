using System;
using System.Collections.Generic;
using System.Reflection;
using GraveyardOrbit.UI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraveyardOrbit.UIScaling
{
    internal static class UIScalePatches
    {
        [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.ApplyAll))]
        internal static class ApplySettingsPatch
        {
            private static void Postfix()
            {
                UiScale.ApplyAll();
            }
        }

        [HarmonyPatch(typeof(SettingsModal), "Open", new Type[]
        {
            typeof(VisualElement),
            typeof(Action),
            typeof(Action)
        })]
        internal static class SettingsUiScaleRowPatch
        {
            private static void Postfix(VisualElement __result)
            {
                AddUiScaleRow(__result);
            }
        }

        private static void AddUiScaleRow(VisualElement modal)
        {
            if (modal == null || modal.Q("ui-scale-row") != null) return;

            try
            {
                Label resolution = FindLabel(
                    modal,
                    Loc.T("settings.display.resolution", "Resolution"));
                if (resolution == null) return;

                VisualElement resolutionRow = resolution.parent;
                VisualElement page = resolutionRow?.parent;
                if (page == null) return;

                MethodInfo makeRow = typeof(SettingsModal).GetMethod(
                    "MakeRow",
                    BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo dropdown = typeof(SettingsModal).GetMethod(
                    "Dropdown",
                    BindingFlags.NonPublic | BindingFlags.Static);
                if (makeRow == null || dropdown == null) return;

                object row = makeRow.Invoke(null, new object[]
                {
                    page,
                    Loc.T("settings.display.text_size", "Text Size"),
                    66f,
                    false,
                    false
                });

                VisualElement rowRoot = row?.GetType()
                    .GetField("root")
                    ?.GetValue(row) as VisualElement;
                if (rowRoot == null) return;

                rowRoot.name = "ui-scale-row";
                int index = page.IndexOf(resolutionRow);
                if (index >= 0) page.Insert(index + 1, rowRoot);

                Func<string[]> options = delegate
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < UiScale.Presets.Length; i++)
                    {
                        int percent = UiScale.Presets[i];
                        list.Add(percent == 200 ? "200% (EXPERIMENTAL)" : percent + "%");
                    }
                    return list.ToArray();
                };

                dropdown.Invoke(null, new object[]
                {
                    row,
                    modal,
                    options,
                    (Func<int>)(() => UiScale.Index),
                    (Action<int>)(i => UiScale.Set(
                        UiScale.Presets[Mathf.Clamp(i, 0, UiScale.Presets.Length - 1)]))
                });
            }
            catch (Exception ex)
            {
                UIScalingPlugin.Log?.LogWarning("Could not add Text Size setting: " + ex);
            }
        }

        private static Label FindLabel(VisualElement root, string text)
        {
            foreach (VisualElement child in root.Children())
            {
                if (child is Label label && label.text == text) return label;
                Label nested = FindLabel(child, text);
                if (nested != null) return nested;
            }
            return null;
        }
    }
}
