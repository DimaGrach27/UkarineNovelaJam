using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ReflectionOfAmber.Scripts.GameScene.BgScreen
{
    public class BgService
    {
        private BgEnum _currentBg = BgEnum.NONE;
        
        private readonly BgUiView _bgUiView;
        private readonly Dictionary<BgEnum, BgScriptableObject> _bgMap = new();
        
        private AnimationBg _currentAnimation;
        private Coroutine _changeBgRoutine;
        
        [Inject]
        public BgService(GamePlayCanvas gamePlayCanvas)
        {
            _bgUiView = gamePlayCanvas.GetComponentInChildren<BgUiView>();

            BgScriptableObject[] bgScriptableObjects =
                Resources.LoadAll<BgScriptableObject>("Configs/BackGrounds");
            
            foreach (var bgScriptable in bgScriptableObjects)
            {
                _bgMap.Add(bgScriptable.Bg, bgScriptable);
            }
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

                if (_bgMap[bgEnum].AnimationScreen.enable)
                {
                    _currentAnimation = Object.Instantiate(_bgMap[bgEnum].AnimationScreen.animationBg, _bgUiView.transform);
                }
            
                _bgUiView.Sprite = _bgMap[bgEnum].Image;
                _bgUiView.Visible = true;
                _currentBg = bgEnum;
                SaveService.SetCurrentBg(bgEnum);
                
                return;
            }
            
            if(_changeBgRoutine != null)
                CoroutineHelper.Inst.StopCoroutine(_changeBgRoutine);

            _changeBgRoutine = CoroutineHelper.Inst.StartCoroutine(ChangeBgRoutine(onDoneAnimation));
        }
        
        public void Hide()
        {
            _bgUiView.Visible = false;
        }
        
        private IEnumerator ChangeBgRoutine(Action onDoneAnima)
        {
            float duration = 3.0f;
            FadeService.FadeService.FadeIn(duration);
            
            yield return new WaitForSeconds(duration);
            
            ScreenSceneScriptableObject currentSceneSo = GameModel.GetScene(SaveService.GetScene);
            
            if(currentSceneSo.ChangeBackGround.enable)
            {
                BgEnum bgEnum = currentSceneSo.ChangeBackGround.bgEnum;
                
                if(_currentAnimation != null) 
                    Object.Destroy(_currentAnimation.gameObject);

                if (_bgMap[bgEnum].AnimationScreen.enable)
                {
                    _currentAnimation = Object.Instantiate(_bgMap[bgEnum].AnimationScreen.animationBg, _bgUiView.transform);
                }
            
                _bgUiView.Sprite = _bgMap[bgEnum].Image;
                _bgUiView.Visible = true;
                _currentBg = bgEnum;
                SaveService.SetCurrentBg(bgEnum);
            }
            
            yield return new WaitForSeconds(1.0f);
            
            FadeService.FadeService.FadeOut(duration);
            
            yield return new WaitForSeconds(duration / 2);
            
            onDoneAnima?.Invoke();
        }
    }
}