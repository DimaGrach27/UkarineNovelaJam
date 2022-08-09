using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction
{
    public class CameraActionFlash : MonoBehaviour
    {
        [SerializeField] private AnimationCurve flashCurve;
        [SerializeField] private Image flashImage;

        private Coroutine _flashRoutine;
        private AudioSystemService _audioSystemService;

        [Inject]
        public void Construct(AudioSystemService audioSystemService)
        {
            _audioSystemService = audioSystemService;
        }
        public void CallFlash()
        {
            if(_flashRoutine != null)
                StopCoroutine(_flashRoutine);

            _flashRoutine = StartCoroutine(FlashRoutine());
        }
        
        private IEnumerator FlashRoutine()
        {
            Color imageColor = GlobalConstant.ColorWitheClear;
            flashImage.gameObject.SetActive(true);
            flashImage.color = imageColor;
            
            _audioSystemService.PlayShotSound(MusicType.PHOTO_CLICK);
            float time = 0.0f;

            while (time < GlobalConstant.CAMERA_ACTION_FLASH_DURATION)
            {
                float alpha = flashCurve.Evaluate(CalculateProgress());
                time += Time.deltaTime;
                
                imageColor.a = alpha;
                flashImage.color = imageColor;
                
                yield return null;
            }
            
            flashImage.gameObject.SetActive(false);

            float CalculateProgress()
            {
                float progress = time / GlobalConstant.CAMERA_ACTION_FLASH_DURATION;
                return progress;
            }
        }
    }
}