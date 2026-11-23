using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace GraveyardOrbit.UIScaling
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class UIScalingPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "agentbox.graveyardorbit.uiscaling";
        public const string PluginName = "Graveyard Orbit UI Scaling";
        public const string PluginVersion = "0.1.0";

        internal static ManualLogSource Log;

        private Harmony _harmony;
        private float _nextScan;

        private void Awake()
        {
            Log = Logger;
            UiScale.Load();

            _harmony = new Harmony(PluginGuid);
            try
            {
                _harmony.PatchAll(typeof(UIScalingPlugin).Assembly);
                // Do not touch Resources here: plugin Awake runs before Unity's
                // resource system is ready. GameSettings.ApplyAll and the
                // UIDocument/CanvasScaler hooks apply the scale once the game
                // has finished booting.
                Log.LogInfo("UI Scaling loaded:");
                Log.LogInfo("  Display settings: Text Size 100 / 125 / 150 / 200%");
            }
            catch (System.Exception ex)
            {
                Log.LogError("UI Scaling patch failure: " + ex);
            }
        }

        private void Update()
        {
            float now = UnityEngine.Time.unscaledTime;
            if (now < _nextScan) return;
            _nextScan = now + 0.5f;
            if (Mathf.Approximately(UiScale.Factor, 1f)) return;
            UiScale.ApplyAll();
        }

        private void OnDestroy()
        {
            try { _harmony?.UnpatchSelf(); } catch { }
        }
    }
}
