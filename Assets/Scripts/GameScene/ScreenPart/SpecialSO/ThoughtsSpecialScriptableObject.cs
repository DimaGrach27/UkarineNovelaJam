﻿using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Thoughts SO", 
        menuName = "Frog Croaked Team/Special/Create 'Thoughts SO'", 
        order = 0)]
    
    public class ThoughtsSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusEnum status = StatusEnum.NONE;
        [SerializeField] private bool valueStatus = true;
        
        public override bool Check()
        {
            return GameModel.GetStatus(status) == valueStatus;
        }
    }
}