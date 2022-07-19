using UnityEngine;

namespace GameScene.BgScreen
{
    [CreateAssetMenu(fileName = "new Bg", menuName = "Frog Croaked Team/Create 'BG'", order = 0)]

    public class BgScriptableObject : ScriptableObject
    {
        [SerializeField] private Sprite image;
        [SerializeField] private BgEnum bgEnum;
        
        public Sprite Image => image;
        public BgEnum Bg => bgEnum;
    }
}