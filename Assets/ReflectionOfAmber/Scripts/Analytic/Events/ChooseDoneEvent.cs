using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class ChooseDoneEvent : Unity.Services.Analytics.Event
    {
        public ChooseDoneEvent() : base("KillerChoose")
        {
            SetParameter("unityPlayerID", AuthenticationService.Instance.PlayerId);
        }

        public string KillerName { set { SetParameter("KillerName", value); } }
    }
}