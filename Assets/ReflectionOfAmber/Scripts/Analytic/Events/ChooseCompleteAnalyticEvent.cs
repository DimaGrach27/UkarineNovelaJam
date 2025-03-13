using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class ChooseCompleteAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "ChooseComplete";
        public Dictionary<string, object> Data { get; }

        public ChooseCompleteAnalyticEvent(string sceneID, int chooseValue)
        {
            Data = new Dictionary<string, object>
            {
                { "SceneID", sceneID },
                { "ChooseValue", chooseValue },
            };
        }
    }
}