using Unity.Services.Authentication;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class TestEvent : Unity.Services.Analytics.Event
    {
        public TestEvent() : base("test_event")
        {
            SetParameter("unityPlayerID", AuthenticationService.Instance.PlayerId);
        }

        public string SceneID { set { SetParameter("SceneID", value); } }
    }
}