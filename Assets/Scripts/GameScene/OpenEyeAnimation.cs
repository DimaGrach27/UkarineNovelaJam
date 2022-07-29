﻿using System;
using System.Collections;
using UnityEngine;

namespace GameScene
{
    public class OpenEyeAnimation : MonoBehaviour
    {
        public static OpenEyeAnimation Inst { get; private set; }

        [SerializeField] private GameObject rayCastImage;
        [SerializeField] private GameObject eyeSprite;
        [SerializeField] private Material eyeMat;
        [SerializeField] private AnimationCurve eyeCurve;
        
        private static readonly int OpenValue = Shader.PropertyToID("_OpenValue");

        private void Awake()
        {
            if(Inst != null) return;

            Inst = this;
        }

        public void PrepareEye()
        {
            StartCoroutine(PrepareEyeRoutine());
        }
        
        private IEnumerator PrepareEyeRoutine()
        {
            yield return new WaitForSeconds(3.0f);
            eyeMat.SetFloat(OpenValue, 0.0f);
            eyeSprite.SetActive(true);
        }
        
        public void PlayOpenEye(Action onDoneAnima)
        {
            StartCoroutine(OpenEyeRoutine(onDoneAnima));
        }
        
        private IEnumerator OpenEyeRoutine(Action onDoneAnima)
        {
            eyeMat.SetFloat(OpenValue, 0.0f);
            eyeSprite.SetActive(true);
            rayCastImage.SetActive(false);
            
            for (float i = 0; i <= 1.0f; i += Time.deltaTime / 5)
            {
                eyeMat.SetFloat(OpenValue, eyeCurve.Evaluate(i));
                yield return null;
            }
            
            onDoneAnima?.Invoke();
            rayCastImage.SetActive(true);
            eyeSprite.SetActive(false);
        }
    }
}