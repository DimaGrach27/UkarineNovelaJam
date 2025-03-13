using ReflectionOfAmber.Scripts.Analytic.Events;

namespace ReflectionOfAmber.Scripts.Analytic
{
    public interface IAnalyticService
    {
        public void Initialize();
        public void TrackEvent(IAnalyticEvent analyticEvent);
    }
}