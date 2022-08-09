using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.Characters
{
    public class CharacterService 
    {
        public CharacterService(Transform uiCharacter)
        {
            foreach (var characterUi in uiCharacter.GetComponentsInChildren<CharacterUiView>())
            {
                _characterUiViewMap.Add(characterUi.ScreenPosition, characterUi);
                _dissolveRoutineMap.Add(characterUi.ScreenPosition, null);
                _dissolveTweenMap.Add(characterUi.ScreenPosition, null);
                _enabledMap.Add(characterUi.ScreenPosition, true);
            }
        }

        private readonly Dictionary<CharacterScreenPositionEnum, CharacterUiView> _characterUiViewMap = new();
        private readonly Dictionary<CharacterScreenPositionEnum, Coroutine> _dissolveRoutineMap = new();
        private readonly Dictionary<CharacterScreenPositionEnum, Tween> _dissolveTweenMap = new();
        private readonly Dictionary<CharacterScreenPositionEnum, bool> _enabledMap = new();

        public void ShowCharacter(CharacterScreenPositionEnum screenPosition, Sprite image)
        {
            HideAllCharacters();
            
            if(image == null) return;
            
            if(image != _characterUiViewMap[screenPosition].Image.sprite)
            {
                HideCharacter(screenPosition, false);
                DissolveIn(screenPosition);
            }
            
            if(_dissolveRoutineMap[screenPosition] != null) 
                CoroutineHelper.Inst.StopCoroutine(_dissolveRoutineMap[screenPosition]);
            
            if(_dissolveTweenMap[screenPosition] != null)
                DOTween.Kill(_dissolveTweenMap[screenPosition]);
            
            _characterUiViewMap[screenPosition].Sprite = image;
            _characterUiViewMap[screenPosition].Visible = true;
            _enabledMap[screenPosition] = true;
        }
        
        private void HideCharacter(CharacterScreenPositionEnum screenPosition, bool isDissolve)
        {
            if(_dissolveRoutineMap[screenPosition] != null)
                CoroutineHelper.Inst.StopCoroutine(_dissolveRoutineMap[screenPosition]);
            
            if(_dissolveTweenMap[screenPosition] != null)
                DOTween.Kill(_dissolveTweenMap[screenPosition]);
            
            if (isDissolve)
            {
                if (_enabledMap[screenPosition])
                    _dissolveRoutineMap[screenPosition] =
                        CoroutineHelper.Inst.StartCoroutine(DissolveOut(screenPosition));
            }
            else
            {
                _characterUiViewMap[screenPosition].Image.sprite = null;
                _characterUiViewMap[screenPosition].Visible = false;
                _enabledMap[screenPosition] = false;
            }
        }
        
        public void HideAllCharacters()
        {
            foreach (var coroutine in _dissolveRoutineMap.Values)
            {
                if(coroutine != null)
                    CoroutineHelper.Inst.StopCoroutine(coroutine);
            }
            
            foreach (var tween in _dissolveTweenMap.Values)
            {
                if(tween != null)
                    DOTween.Kill(tween);
            }
            
            foreach (var character in _characterUiViewMap)
            {
                if (_enabledMap[character.Key])
                    _dissolveRoutineMap[character.Key] =
                        CoroutineHelper.Inst.StartCoroutine(DissolveOut(character.Key));
            }
        }

        public void HideAllCharactersInstant()
        {
            foreach (var character in _characterUiViewMap.Values)
            {
                character.Image.sprite = null;
                character.Visible = false;
            }
        }
        
        private void DissolveIn(CharacterScreenPositionEnum screenPosition, 
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            _characterUiViewMap[screenPosition].Image.color = GlobalConstant.ColorWitheClear;
            _dissolveTweenMap[screenPosition] = 
                _characterUiViewMap[screenPosition].Image.DOFade(1.0f, duration);
        }
        
        private IEnumerator DissolveOut(CharacterScreenPositionEnum screenPosition, 
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            yield return null;
            
            _characterUiViewMap[screenPosition].Image.color = Color.white;
            _dissolveTweenMap[screenPosition] = 
                _characterUiViewMap[screenPosition].Image.DOFade(0.0f, duration);
            
            yield return new WaitForSeconds(duration);
            
            _characterUiViewMap[screenPosition].Visible = false;
            _enabledMap[screenPosition] = false;
            _characterUiViewMap[screenPosition].Image.sprite = null;
        }
    }
}
