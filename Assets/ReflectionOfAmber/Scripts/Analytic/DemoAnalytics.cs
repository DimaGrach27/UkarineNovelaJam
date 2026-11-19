using System.Collections.Generic;
using ReflectionOfAmber.Scripts.Analytic.Events;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Analytic
{
    // Created only by the ANALYTIC_ENABLED service. Calls are harmless in other builds.
    public sealed class DemoAnalytics : MonoBehaviour
    {
        private static DemoAnalytics instance;
        private AnalyticService service;
        private float activeSeconds, checkpointAt;
        private readonly string sessionId = System.Guid.NewGuid().ToString("N");
        private int runNumber;
        private bool paused;
        public static void SetPaused(bool value) { if (instance != null) instance.paused = value; }
        private bool started;
        private readonly Dictionary<string, string> settings = new();
        private readonly Dictionary<string, float> changedAt = new();
        private readonly Dictionary<string, string> reported = new();
        public static float ActiveSeconds => instance == null ? 0 : instance.activeSeconds;

        public static void Create(AnalyticService service)
        {
            if (instance != null) return;
            instance = new GameObject("DemoAnalytics").AddComponent<DemoAnalytics>();
            instance.service = service;
            DontDestroyOnLoad(instance.gameObject);
        }

        public static void Track(string name, string path = null, float? value = null,
            Dictionary<string, object> data = null)
        {
            if (instance == null || instance.service == null) return;
            if (name == "NewGameStarted")
            {
                instance.runNumber++;
                PlayerPrefs.SetInt("AnalyticsRunNumber", instance.runNumber);
                PlayerPrefs.Save();
            }
            data ??= new Dictionary<string, object>();
            data["EventPath"] = path ?? name;
            data["SessionID"] = instance.sessionId;
            data["RunNumber"] = instance.runNumber;
            data["BuildVersion"] = Application.version;
            data["Language"] = GameModel.CurrentLanguage.ToString();
            data["Platform"] = Application.platform.ToString();
            data["IsEditor"] = Application.isEditor;
            data["IsDevelopmentBuild"] = Debug.isDebugBuild;
            instance.service.ReportEvent(new GameplayAnalyticEvent(name, path ?? name, data, value));
        }

        private void Start()
        {
            runNumber = PlayerPrefs.GetInt("AnalyticsRunNumber", 0);
            ReadInitialSettings();
            foreach (var pair in settings)
            {
                reported[pair.Key] = pair.Value;
                SendSetting("SettingUsed", pair.Key, pair.Value);
            }
            changedAt.Clear();
            started = true;
            Track("DemoSessionStarted");
        }

        private void Update()
        {
            if (Application.isFocused && GameModel.IsGamePlaying && !paused)
                activeSeconds += Time.unscaledDeltaTime;
            if (changedAt.Count > 0)
                FlushSettings(false);
            if (activeSeconds - checkpointAt >= 30)
            {
                Checkpoint("interval");
                checkpointAt = activeSeconds;
            }
        }

        private void ReadInitialSettings()
        {
            Observe("language", SaveService.LanguageStatus.ToString());
            Observe("dialog_keyboard_mouse", SaveService.DialogKeyboardMouseEnabled ? "on" : "off");
            Observe("text_speed", Bucket(SaveService.TypingSpeed));
            Observe("music_volume", Bucket(SaveService.MusicVolume));
            Observe("sound_volume", Bucket(SaveService.AudioVolume));
            Observe("brightness", Bucket(SaveService.BrightnessValue));
        }

        // Called only after SaveService has persisted the setting. Startup writes are
        // represented by the initial snapshot, not by user-change events.
        public static void SettingChanged(string key, float value)
        {
            if (instance == null || !instance.started) return;
            instance.Observe(key, Bucket(value));
        }

        public static void SettingChanged(string key, string value)
        {
            if (instance == null || !instance.started) return;
            instance.Observe(key, value);
        }

        private static string Bucket(float value) => Mathf.RoundToInt(value * 10).ToString(System.Globalization.CultureInfo.InvariantCulture);
        private void Observe(string key, string value)
        {
            if (settings.TryGetValue(key, out var previous) && previous == value) return;
            settings[key] = value;
            changedAt[key] = Time.realtimeSinceStartup;
        }

        private static void SendSetting(string name, string key, string value)
        {
            Track(name, $"{name}:{key}:{value}", data: new Dictionary<string, object>
            { { "Setting", key }, { "SettingValue", value } });
        }

        private void FlushSettings(bool force)
        {
            foreach (var key in new List<string>(changedAt.Keys))
            {
                if (!force && Time.realtimeSinceStartup - changedAt[key] < 1) continue;
                if (!reported.TryGetValue(key, out var old) || old != settings[key])
                    SendSetting("SettingChanged", key, settings[key]);
                reported[key] = settings[key];
                changedAt.Remove(key);
            }
        }

        private void Checkpoint(string reason)
        {
            Track("ProgressCheckpoint", $"ProgressCheckpoint:{SaveService.GetScene}:{reason}", activeSeconds,
                new Dictionary<string, object> { { "SceneID", SaveService.GetScene },
                    { "PartIndex", SaveService.GetPart }, { "ActiveSeconds", activeSeconds }, { "Reason", reason } });
        }
        private void OnApplicationFocus(bool focus)
        {
            if (!started || focus) return;
            FlushSettings(true);
            Checkpoint("focus_lost");
        }
        private void OnApplicationQuit()
        {
            if (!started) return;
            FlushSettings(true);
            Checkpoint("quit");
        }
        private void OnDestroy() { if (instance == this) instance = null; }
    }
}
