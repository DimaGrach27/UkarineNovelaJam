using System;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart
{
    public class ScreenPartsServiceFacade
    {
        public event Action<string, int> OnPlayNextScene;
        public event Action OnPlayNextPart;

        public void PlayNextScene(string sceneKey, int part = 0)
        {
            OnPlayNextScene?.Invoke(sceneKey, part);
        }

        public void PlatNextPart()
        {
            OnPlayNextPart?.Invoke();
        }
    }
}