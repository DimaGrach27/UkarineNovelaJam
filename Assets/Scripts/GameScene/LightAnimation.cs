using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace GameScene
{
    public class LightAnimation : MonoBehaviour
    {
        private Light2D _light2D;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
            StartCoroutine(LightAnimaRoutine());
        }

        private IEnumerator LightAnimaRoutine()
        {
            int blinkCount = Random.Range(1, 5);
            float intervalBlink = Random.Range(0.0f, 0.5f);
            float intervalDelay = Random.Range(3, 10);
            float minIntensity = Random.value;
            
            _light2D.intensity = 1.0f;
            
            yield return new WaitForSeconds(intervalDelay);
            
            while (true)
            {
                for (int i = 0; i < blinkCount; i++)
                {
                    for (float j = 1.0f; j >= minIntensity; j -= Time.deltaTime * 4.0f)
                    {
                        _light2D.intensity = j;
                        yield return null;
                    }
                    
                    for (float j = minIntensity; j <= 1.0f; j += Time.deltaTime * 4.0f)
                    {
                        _light2D.intensity = j;
                        yield return null;
                    }
                    
                    yield return new WaitForSeconds(intervalBlink);
                    
                    minIntensity = Random.value;
                    intervalBlink = Random.Range(0.0f, 0.5f);
                }
                
                _light2D.intensity = 1.0f;
                
                yield return new WaitForSeconds(intervalDelay);
                
                blinkCount = Random.Range(1, 5);
                intervalDelay = Random.Range(7, 16);
            }
        }
    }
}