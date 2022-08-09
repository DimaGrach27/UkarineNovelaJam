using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReflectionOfAmber.Scripts.GameScene
{
    public class LightAnimation : MonoBehaviour
    {
        // private Light2D _light2D;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // _light2D = GetComponent<Light2D>();
            StartCoroutine(LightAnimaRoutine());
        }

        private IEnumerator LightAnimaRoutine()
        {
            int blinkCount = Random.Range(1, 5);
            float intervalBlink = Random.Range(0.0f, 0.5f);
            float intervalDelay = Random.Range(3, 10);
            float minIntensity = Random.value;
            
            // _light2D.intensity = 1.0f;
            Color colorLight = Color.white;
            _spriteRenderer.color = colorLight;
            
            yield return new WaitForSeconds(intervalDelay);
            
            while (true)
            {
                for (int i = 0; i < blinkCount; i++)
                {
                    for (float j = 1.0f; j >= minIntensity; j -= Time.deltaTime * 4.0f)
                    {
                        // _light2D.intensity = j;
                        colorLight.a = j;
                        _spriteRenderer.color = colorLight;

                        yield return null;
                    }
                    
                    for (float j = minIntensity; j <= 1.0f; j += Time.deltaTime * 4.0f)
                    {
                        // _light2D.intensity = j;
                        colorLight.a = j;
                        _spriteRenderer.color = colorLight;
                        
                        yield return null;
                    }
                    
                    yield return new WaitForSeconds(intervalBlink);
                    
                    minIntensity = Random.value;
                    intervalBlink = Random.Range(0.0f, 0.5f);
                }
                
                // _light2D.intensity = 1.0f;
                colorLight = Color.white;
                _spriteRenderer.color = colorLight;
                
                yield return new WaitForSeconds(intervalDelay);
                
                blinkCount = Random.Range(1, 5);
                intervalDelay = Random.Range(7, 16);
            }
        }
    }
}