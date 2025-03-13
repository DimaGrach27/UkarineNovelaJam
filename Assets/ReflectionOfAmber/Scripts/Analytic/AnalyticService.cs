using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using ReflectionOfAmber.Scripts.Analytic.Events;
using ReflectionOfAmber.Scripts.GameModelBlock;
using Unity.Services.Analytics;
using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public class AnalyticService : IInit
    {
        public event Action OnReady;
        public void Init()
        {
            GameAnalytics.SetCustomId(AuthenticationService.Instance.PlayerId);
            GameAnalytics.Initialize();
            
            GameAnalytics.NewDesignEvent("Scenes:completed_scene", new Dictionary<string, object>()
            {
                {"SceneID", "TestScene"},
            });
            
            
            AnalyticsService.Instance.StartDataCollection();
            AnalyticsService.Instance.RecordEvent(new TestEvent()
            {
                SceneID = "TestScene",
            });
            
            OnReady?.Invoke();
        }
    }
}