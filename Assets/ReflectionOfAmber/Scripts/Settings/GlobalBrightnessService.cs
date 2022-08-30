using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ReflectionOfAmber.Scripts.Settings
{
    public class GlobalBrightnessService : MonoBehaviour
    {
        private Volume Volume => _volume ??= GetComponent<Volume>();
        private Volume _volume;
        
        private ColorAdjustments ColorAdjustments => _colorAdjustments ??= Volume.profile.components[0] as ColorAdjustments;
        private ColorAdjustments _colorAdjustments;

        public float BrightnessValue
        {
            set
            {
                float calculate = value;
                
                calculate *= 4.0f;
                calculate -= 2.0f;
                
                ColorAdjustments.postExposure.value = calculate;
            }
        }
    }
}