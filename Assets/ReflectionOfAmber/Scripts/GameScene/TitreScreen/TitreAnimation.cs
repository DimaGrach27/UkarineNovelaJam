using System;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.TitreScreen
{
    public class TitreAnimation : MonoBehaviour
    {
        public event Action OnEndAnimation;
        
        private void OnAnimationEnd()
        {
            OnEndAnimation?.Invoke();
            Destroy(gameObject);
        }
    }
}