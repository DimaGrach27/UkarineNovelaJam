using System.Collections.Generic;
using UnityEngine;

namespace GameScene.BgScreen
{
    public class BgService
    {
        private readonly BgUiView _bgUiView;
        private readonly Dictionary<BgEnum, BgScriptableObject> _bgMap = new();

        public BgService(Transform uiTransform)
        {
            _bgUiView = uiTransform.GetComponentInChildren<BgUiView>();

            BgScriptableObject[] bgScriptableObjects =
                Resources.LoadAll<BgScriptableObject>("Configs/BackGrounds");
            
            foreach (var bgScriptable in bgScriptableObjects)
            {
                _bgMap.Add(bgScriptable.Bg, bgScriptable);
            }
        }

        public void Show(BgEnum bgEnum)
        {
            _bgUiView.Image = _bgMap[bgEnum].Image;
            _bgUiView.Visible = true;
        }
        
        public void Hide()
        {
            _bgUiView.Visible = false;
        }
    }
}