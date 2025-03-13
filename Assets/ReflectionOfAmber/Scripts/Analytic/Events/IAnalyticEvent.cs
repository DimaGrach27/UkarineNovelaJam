using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public interface IAnalyticEvent
    {
        public string EventName { get; }
        public Dictionary<string, object> Data { get; }
    }
}