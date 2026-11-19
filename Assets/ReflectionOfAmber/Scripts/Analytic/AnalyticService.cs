using System;
using System.Collections.Generic;
using UnityEngine;
using ReflectionOfAmber.Scripts.Analytic.Events;
using ReflectionOfAmber.Scripts.GlobalProject;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public class AnalyticService : IInit
    {
        private IAnalyticService[] m_AnalyticsServices;
        private readonly HashSet<Type> reportedFailures = new();
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
            
            DemoAnalytics.Create(this);
            OnReady?.Invoke();
        }

        public void ReportEvent(IAnalyticEvent analyticEvent)
        {
            foreach (var analyticsService in m_AnalyticsServices)
            {
                try
                {
                    analyticsService.TrackEvent(analyticEvent);
                }
                catch (Exception exception)
                {
                    // Telemetry failures must not interrupt a choice or prevent the other provider receiving it.
                    if (reportedFailures.Add(analyticsService.GetType()))
                        Debug.LogWarning($"Analytics provider {analyticsService.GetType().Name} failed: {exception.Message}");
                }
            }
        }
    }
}