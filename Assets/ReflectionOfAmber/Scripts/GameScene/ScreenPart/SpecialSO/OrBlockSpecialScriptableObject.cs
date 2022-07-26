﻿using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Or SO", 
        menuName = "Frog Croaked Team/Special/Create 'Or SO'", 
        order = 0)]
    
    public class OrBlockSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusEnum statusEnum;
        
        [SerializeField] private NextScene blockTrue;
        [SerializeField] private NextScene blockFalse;

        private NextScene _nextScene;
        
        public override bool Check()
        {
            bool state = SaveService.GetStatusValue(statusEnum);

            _nextScene = state ? blockTrue : blockFalse;
            return true;
        }

        public override NextScene NextScene => _nextScene;
    }
}