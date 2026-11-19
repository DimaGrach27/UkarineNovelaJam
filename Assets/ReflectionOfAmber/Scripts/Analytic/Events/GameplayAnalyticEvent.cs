using System.Collections.Generic;
namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    // Explicit GA path: numeric values and session context must never become event IDs.
    public sealed class GameplayAnalyticEvent : IAnalyticEvent
    {
        public string EventName { get; }
        public Dictionary<string, object> Data { get; }
        public string DesignPath { get; }
        public float? Value { get; }
        public GameplayAnalyticEvent(string name, string path, Dictionary<string, object> data, float? value = null)
        {
            EventName = name;
            DesignPath = path;
            Data = data;
            Value = value;
        }
    }
}
