using System;
using ReflectionOfAmber.Scripts.Analytic.Events;
using ReflectionOfAmber.Scripts.GlobalProject;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public class AnalyticService : IInit
    {
        private IAnalyticService[] m_AnalyticsServices;
        public event Action OnReady;
        public void Init()
        {
            m_AnalyticsServices = new IAnalyticService[]
            {
                new UnityAnalyticService(),
                new GameAnalyticsService(),
            };
            
            foreach (var analyticsService in m_AnalyticsServices)
            {
                analyticsService.Initialize();
            }
            
            OnReady?.Invoke();
        }

        public void ReportEvent(IAnalyticEvent analyticEvent)
        {
            foreach (var analyticsService in m_AnalyticsServices)
            {
                analyticsService.TrackEvent(analyticEvent);
            }
        }
    }
}