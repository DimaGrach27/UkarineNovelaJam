using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GameScene.GlobalVolume
{
    public class GlobalVolumeService : MonoBehaviour
    {
        public static GlobalVolumeService Inst { get; private set; }

        private Volume _volume;

        [SerializeField] private AnimationCurve aberrationPulseCurve;
        [SerializeField] private AnimationCurve vignetteEyeCurve;

        private ChromaticAberration _chromaticAberration;
        private Vignette _vignette;
        
        private void Awake()
        {
            if(Inst != null) return;

            Inst = this;
            _volume = GetComponent<Volume>();

            _chromaticAberration = _volume.profile.components[0] as ChromaticAberration;
            _vignette = _volume.profile.components[2] as Vignette;

            // StartCoroutine(AberrationPulseRoutine());
        }

        [ContextMenu("Open Eye")]
        public void PlayOpenEye()
        {
            StartCoroutine(OpenEyeRoutine());
        }

        private IEnumerator AberrationPulseRoutine()
        {
            while (true)
            {
                for (float i = 0; i <= 1.0f; i += Time.deltaTime)
                {
                    _chromaticAberration.intensity.value = aberrationPulseCurve.Evaluate(i);
                    yield return null;
                }
                
                yield return new WaitForSeconds(2.0f);
            }
        }
        
        private IEnumerator OpenEyeRoutine()
        {
            _vignette.intensity.value = 1.0f;
            _vignette.active = true;
            
            for (float i = 0; i <= 3.0f; i += Time.deltaTime)
            {
                _vignette.intensity.value = vignetteEyeCurve.Evaluate(i);
                yield return null;
            }
            
            _vignette.active = false;
        }
    }
}