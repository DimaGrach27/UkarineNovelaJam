﻿using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ReflectionOfAmber.Scripts.GameScene.BgScreen
{
    public class BgService
    {
        private BgEnum _currentBg = BgEnum.NONE;
        
        private readonly CoroutineHelper _coroutineHelper;
        private readonly BgUiView _bgUiView;
        private readonly FadeService _fadeService;
        
        private AnimationBg _currentAnimation;
        private Coroutine _changeBgRoutine;
        
        [Inject]
        public BgService(GamePlayCanvas gamePlayCanvas, 
            CoroutineHelper coroutineHelper,
            FadeService fadeService)
        {
            _bgUiView = gamePlayCanvas.GetComponentInChildren<BgUiView>();
            _coroutineHelper = coroutineHelper;
            _fadeService = fadeService;
        }

        public void Show(BgEnum bgEnum, Action onDoneAnimation)
        {
            if(bgEnum == _currentBg)
            {
                onDoneAnimation?.Invoke();
                return;
            }

            if (onDoneAnimation == null)
            {
                if(_currentAnimation != null) 
                    Object.Destroy(_currentAnimation.gameObject);

                AnimationScreen animationScreen = GameModel.GetAnimationScreen(bgEnum);
                
                if (animationScreen.enable)
                {
                    _currentAnimation = Object.Instantiate(animationScreen.animationBg, _bgUiView.transform);
                }
            
                _bgUiView.Sprite = GameModel.GetBg(bgEnum);
                _bgUiView.Visible = true;
                _currentBg = bgEnum;
                SaveService.SetCurrentBg(bgEnum);
                
                return;
            }
            
            if(_changeBgRoutine != null)
                _coroutineHelper.StopCoroutine(_changeBgRoutine);

            _changeBgRoutine = _coroutineHelper.StartCoroutine(ChangeBgRoutine(onDoneAnimation));
        }
        
        public void Hide()
        {
            _bgUiView.Visible = false;
        }
        
        
        private IEnumerator ChangeBgRoutine(Action onDoneAnima)
        {
            float duration = 3.0f;
            _fadeService.FadeIn(duration);
            
            yield return new WaitForSeconds(duration);
            
            ScreenSceneScriptableObject currentSceneSo = GameModel.GetScene(SaveService.GetScene);
            
            if(currentSceneSo.ChangeBackGround.enable)
            {
                BgEnum bgEnum = currentSceneSo.ChangeBackGround.bgEnum;
                
                if(_currentAnimation != null) 
                    Object.Destroy(_currentAnimation.gameObject);

                AnimationScreen animationScreen = GameModel.GetAnimationScreen(bgEnum);
                
                if (animationScreen.enable)
                {
                    _currentAnimation = Object.Instantiate(animationScreen.animationBg, _bgUiView.transform);
                }
            
                _bgUiView.Sprite = GameModel.GetBg(bgEnum);
                _bgUiView.Visible = true;
                _currentBg = bgEnum;
                SaveService.SetCurrentBg(bgEnum);
            }
            
            yield return new WaitForSeconds(1.0f);
            
            _fadeService.FadeOut(duration);
            
            yield return new WaitForSeconds(duration / 2);
            
            onDoneAnima?.Invoke();
        }
    }
}