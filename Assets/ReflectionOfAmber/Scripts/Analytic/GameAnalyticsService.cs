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
                case GameplayAnalyticEvent gameplay:
                    if (gameplay.Value.HasValue)
                        GameAnalytics.NewDesignEvent(gameplay.DesignPath, gameplay.Value.Value, gameplay.Data);
                    else
                        GameAnalytics.NewDesignEvent(gameplay.DesignPath, gameplay.Data);
                    break;

                case StartSceneAnalyticEvent @event:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,@event.Data["SceneID"].ToString());
                    break;
                
                case EndSceneAnalyticEvent @event:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, @event.Data["SceneID"].ToString());
                    break;
                
                default:
                    SendDesignEvent(analyticEvent);
                    break;
            }
        }

        private void SendDesignEvent(IAnalyticEvent analyticEvent)
        {
            string eventData = analyticEvent.EventName;

            foreach (var data in analyticEvent.Data.Values)
            {
                eventData += $":{data}";
            }
            
            GameAnalytics.NewDesignEvent(eventData);
        }
    }
}