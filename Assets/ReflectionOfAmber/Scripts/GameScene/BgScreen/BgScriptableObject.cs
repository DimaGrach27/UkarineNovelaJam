using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.BgScreen
{
    [CreateAssetMenu(fileName = "new Bg", menuName = "Frog Croaked Team/Create 'BG'", order = 0)]

    public class BgScriptableObject : ScriptableObject
    {
        [SerializeField] private Sprite image;
        [SerializeField] private BgEnum bgEnum;
        [SerializeField, Toggle("enable")] private AnimationScreen animationScreen;
        
        public AnimationScreen AnimationScreen => animationScreen;
        public Sprite Image => image;
        public BgEnum Bg => bgEnum;
    }

    [Serializable]
    public class AnimationScreen
    {
        public bool enable;
        public AnimationBg animationBg;
    }
}