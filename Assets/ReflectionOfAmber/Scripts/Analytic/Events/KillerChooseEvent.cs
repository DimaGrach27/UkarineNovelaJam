using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class KillerChooseEvent : Unity.Services.Analytics.Event
    {
        public KillerChooseEvent() : base("Choosen_done")
        {
            SetParameter("unityPlayerID", AuthenticationService.Instance.PlayerId);
        }
    }
}