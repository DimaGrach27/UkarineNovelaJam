using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class SceneCompleteEvent : Unity.Services.Analytics.Event
    {
        public SceneCompleteEvent() : base("CompleteScene")
        {
            SetParameter("unityPlayerID", AuthenticationService.Instance.PlayerId);
        }
        
        public string SceneID { set { SetParameter("SceneID", value); } }
    }
}