using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GraveyardOrbit.UIScaling
{
    /// <summary>
    /// Scales text only. Changing the PanelSettings reference resolution also
    /// moves fixed-position UI around, which pushed the main-menu options
    /// offscreen on the Deck. Font sizes can grow without changing the layout.
    /// </summary>
    internal static class UiScale
    {
        internal static readonly int[] Presets = { 100, 125, 150, 200 };

        private const string PrefKey = "go_settings_ui_scale";
        private const int DefaultPercent = 100;

        private sealed class FontBase
        {
            internal float Size;
            internal float AppliedFactor = 1f;
        }

        private static readonly ConditionalWeakTable<TextElement, FontBase> TextBases =
            new ConditionalWeakTable<TextElement, FontBase>();

        private static readonly ConditionalWeakTable<Text, FontBase> UguiTextBases =
            new ConditionalWeakTable<Text, FontBase>();

        internal static int Current { get; private set; } = DefaultPercent;

        internal static float Factor => Current / 100f;

        internal static int Index
        {
            get
            {
                for (int i = 0; i < Presets.Length; i++)
                {
                    if (Presets[i] == Current) return i;
                }
                return 0;
            }
        }

        internal static void Load()
        {
            int value = PlayerPrefs.GetInt(PrefKey, DefaultPercent);
            if (!IsPreset(value)) value = DefaultPercent;
            Current = value;
        }

        internal static void Set(int percent)
        {
            if (!IsPreset(percent)) percent = DefaultPercent;
            if (Current == percent) return;

            Current = percent;
            PlayerPrefs.SetInt(PrefKey, percent);
            PlayerPrefs.Save();
            ApplyAll();
        }

        private static bool IsPreset(int value)
        {
            for (int i = 0; i < Presets.Length; i++)
            {
                if (Presets[i] == value) return true;
            }
            return false;
        }

        internal static void ApplyAll()
        {
            float factor = Factor;

            foreach (UIDocument document in Resources.FindObjectsOfTypeAll<UIDocument>())
            {
                VisualElement root = document?.rootVisualElement;
                if (root != null) ScaleTree(root, factor);
            }

            foreach (Text text in Resources.FindObjectsOfTypeAll<Text>())
            {
                if (text == null || !text.gameObject.scene.IsValid()) continue;
                ScaleText(text, factor);
            }
        }

        private static void ScaleTree(VisualElement element, float factor)
        {
            if (element is TextElement text)
            {
                ScaleTextElement(text, factor);
            }

            foreach (VisualElement child in element.Children())
            {
                ScaleTree(child, factor);
            }
        }

        private static void ScaleTextElement(TextElement element, float factor)
        {
            float current = element.resolvedStyle.fontSize;
            if (current <= 0.01f) return;

            FontBase baseline = TextBases.GetValue(element, delegate
            {
                return new FontBase { Size = current };
            });

            // A screen can re-use a label and assign a new font size after we
            // have touched it. Detect that and rebase instead of compounding.
            if (Mathf.Abs(current - baseline.Size * baseline.AppliedFactor) > 0.51f)
            {
                baseline.Size = current / Mathf.Max(0.01f, baseline.AppliedFactor);
            }

            // Contract cards and the extractor's 200x64 purchase cards are
            // fixed-size and cannot absorb larger type without overrunning.
            // These screens are not text-reading critical, so keep their text
            // at the original size.
            float applied = IsFixedSizeCard(element) ? 1f : factor;

            // The base title is centered beside the shard counters. Capping it
            // slightly keeps "Corporate Outpost" clear of them.
            if (element.ClassListContains("top-hud__title"))
            {
                applied = Mathf.Min(applied, 1.25f);
            }
            element.style.fontSize = baseline.Size * applied;
            baseline.AppliedFactor = applied;
        }

        private static bool IsFixedSizeCard(VisualElement element)
        {
            for (VisualElement e = element; e != null; e = e.parent)
            {
                if (e.ClassListContains("term-door")) return true;
                if (e.name != null
                    && e.name.StartsWith("tray-")
                    && e.name != "tray-price")
                {
                    return true;
                }
            }
            return false;
        }

        private static void ScaleText(Text text, float factor)
        {
            int current = text.fontSize;
            if (current <= 0) return;

            FontBase baseline = UguiTextBases.GetValue(text, delegate
            {
                return new FontBase { Size = current };
            });

            if (Mathf.Abs(current - baseline.Size * baseline.AppliedFactor) > 0.51f)
            {
                baseline.Size = current / Mathf.Max(0.01f, baseline.AppliedFactor);
            }

            text.fontSize = Mathf.Max(1, Mathf.RoundToInt(baseline.Size * factor));
            baseline.AppliedFactor = factor;
        }
    }
}
