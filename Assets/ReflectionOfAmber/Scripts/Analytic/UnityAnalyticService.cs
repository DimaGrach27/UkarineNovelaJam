using ReflectionOfAmber.Scripts.Analytic.Events;
using Unity.Services.Analytics;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public class UnityAnalyticService : IAnalyticService
    {
        public void Initialize()
        {
            AnalyticsService.Instance.StartDataCollection();
        }

        public void TrackEvent(IAnalyticEvent analyticEvent)
        {
            CustomEvent customEvent = new CustomEvent(analyticEvent.EventName);
            foreach ((string key, object obj) in analyticEvent.Data)
            {
                customEvent.Add(key, obj);
            }
            
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
}