using GameAnalyticsSDK;
using ReflectionOfAmber.Scripts.Analytic.Events;
using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public class GameAnalyticsService : IAnalyticService
    {
        public void Initialize()
        {
            GameAnalytics.SetCustomId(AuthenticationService.Instance.PlayerId);
            GameAnalytics.Initialize();
        }

        public void TrackEvent(IAnalyticEvent analyticEvent)
        {
            switch (analyticEvent)
            {
                case StartSceneAnalyticEvent @event:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, @event.EventName, @event.Data);
                    break;
                
                case EndSceneAnalyticEvent @event:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, @event.EventName, @event.Data);
                    break;
                
                default:
                    GameAnalytics.NewDesignEvent(analyticEvent.EventName, analyticEvent.Data);
                    break;
            }
        }
    }
}