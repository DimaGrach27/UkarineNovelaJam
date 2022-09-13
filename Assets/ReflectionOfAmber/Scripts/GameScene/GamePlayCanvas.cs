using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene
{
    public class GamePlayCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] groups;

        private void Awake()
        {
            GlobalEvent.OnShowCanvas += ShowCanvas;
            GlobalEvent.OnHideCanvas += HideCanvas;
        }

        private void OnDestroy()
        {
            GlobalEvent.OnShowCanvas -= ShowCanvas;
            GlobalEvent.OnHideCanvas -= HideCanvas;
        }

        private void ShowCanvas()
        {
            foreach (var canvasGroup in groups)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        private void HideCanvas()
        {
            foreach (var canvasGroup in groups)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}