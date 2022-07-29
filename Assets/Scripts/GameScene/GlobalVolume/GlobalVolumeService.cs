using System;
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

        private ChromaticAberration _chromaticAberration;
        
        private void Awake()
        {
            if(Inst != null) return;

            Inst = this;
            _volume = GetComponent<Volume>();

            _chromaticAberration = _volume.profile.components[0] as ChromaticAberration;

            StartCoroutine(AberrationPulseRoutine());
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
    }
}