using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class EndSceneAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "EndScene";
        public Dictionary<string, object> Data { get; }
        
        public EndSceneAnalyticEvent(string sceneId)
        {
            Data = new Dictionary<string, object>()
            {
                { "SceneID", sceneId }
            };
        }
    }
}