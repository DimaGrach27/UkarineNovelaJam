﻿using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.BgScreen
{
    public class BgUiView : MonoBehaviour
    {
        [SerializeField] private Image bgCurrent;
        [SerializeField] private Image bgNew;
        
        public Sprite Image
        {
            set
            {
                if (bgCurrent.sprite != value)
                {
                    StartCoroutine(ShowNext(value));
                }
            }
        }

        private IEnumerator ShowNext(Sprite newBg)
        {
            bgNew.gameObject.SetActive(true);
            bgNew.sprite = newBg;
            bgNew.color = GlobalConstant.ColorWitheClear;
            bgNew.DOFade(1.0f, GlobalConstant.ANIMATION_DISSOLVE_DURATION);
            
            yield return new WaitForSeconds(GlobalConstant.ANIMATION_DISSOLVE_DURATION);
            
            bgCurrent.sprite = newBg;
            
            bgNew.gameObject.SetActive(false);
        }
        
        public bool Visible
        {
            set => gameObject.SetActive(value);
        }
    }
}