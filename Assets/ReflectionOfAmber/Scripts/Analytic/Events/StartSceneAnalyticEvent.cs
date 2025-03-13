using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class StartSceneAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "StartScene";
        public Dictionary<string, object> Data { get; }
        
        public StartSceneAnalyticEvent(string sceneId)
        {
            Data = new Dictionary<string, object>()
            {
                { "SceneId", sceneId }
            };
        }
    }
}