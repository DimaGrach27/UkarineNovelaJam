using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class EndDemoAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "EndDemoGame";
        public Dictionary<string, object> Data => new Dictionary<string, object>();
    }
}