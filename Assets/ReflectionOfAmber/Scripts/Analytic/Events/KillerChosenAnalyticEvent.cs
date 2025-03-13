using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class KillerChosenAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "KillerChosen";
        public Dictionary<string, object> Data { get; }

        public KillerChosenAnalyticEvent(KillerName killerName)
        {
            Data = new Dictionary<string, object>()
            {
                {"KillerName", killerName.ToString()}
            };
        }
    }
}