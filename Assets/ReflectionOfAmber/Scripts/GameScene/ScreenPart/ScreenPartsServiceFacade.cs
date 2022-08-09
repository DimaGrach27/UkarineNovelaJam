using System;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart
{
    public class ScreenPartsServiceFacade
    {
        public event Action<string> OnPlayNextScene;
        public event Action OnPlayNextPart;

        public void PlayNextScene(string sceneKey)
        {
            OnPlayNextScene?.Invoke(sceneKey);
        }

        public void PlatNextPart()
        {
            OnPlayNextPart?.Invoke();
        }
    }
}