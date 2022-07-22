using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MainMenu
{
    public class BirdShadowAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer animationPrefab;
        [SerializeField] private Sprite[] spriteAtlas;
        
        [SerializeField] private Transform maxY;
        [SerializeField] private Transform minY;
        
        [SerializeField] private Transform startX;
        [SerializeField] private Transform endX;

        [SerializeField] private float minDuration;
        [SerializeField] private float maxDuration;
        
        [SerializeField, Range(1, 10)] private int maxAnimaPart;

        private Transform _currentAnima;
        private Coroutine _coroutine;
        
        [ContextMenu("Start Anima")]
        public void StartAnima()
        {
            _coroutine = StartCoroutine(AnimationRoutine());
        }
        
        [ContextMenu("Stop Anima")]
        public void StopAnima()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator AnimationRoutine()
        {
            while (true)
            {
                float duration = Random.Range(minDuration, maxDuration);
                float y = Random.Range(minY.position.y, maxY.position.y);
                
                int randomIndex = Random.Range(0, spriteAtlas.Length);
                
                Vector3 positionSpawn = new Vector3(startX.position.x, y);

                Sprite sprite = spriteAtlas[randomIndex];
                
                SpriteRenderer spriteRenderer = 
                    Instantiate(animationPrefab, positionSpawn, Quaternion.identity);
                spriteRenderer.sprite = sprite;

                _currentAnima = spriteRenderer.transform;
                
                _currentAnima.DOMoveX(endX.position.x, duration).SetEase(Ease.Linear);
             
                yield return new WaitForSeconds(duration);
                
                Destroy(_currentAnima.gameObject);
            }
        }
    }
}