using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Characters
{
    public class CharacterService 
    {
        private readonly Dictionary<CharacterScreenPositionEnum, CharacterUiView> _characterUiViewMap = new();

        public CharacterService(Transform uiCharacter)
        {
            foreach (var characterUi in uiCharacter.GetComponentsInChildren<CharacterUiView>())
            {
                _characterUiViewMap.Add(characterUi.ScreenPosition, characterUi);
            }
        }

        public void ShowCharacter(CharacterScreenPositionEnum screenPosition, Sprite image)
        {
            HideAllCharacters();
            _characterUiViewMap[screenPosition].Image = image;
        }
        
        public void HideCharacter(CharacterScreenPositionEnum screenPosition)
        {
            _characterUiViewMap[screenPosition].Visible = false;
        }
        
        public void HideAllCharacters()
        {
            foreach (var character in _characterUiViewMap.Values)
            {
                character.Visible = false;
            }
        }
    }
}
