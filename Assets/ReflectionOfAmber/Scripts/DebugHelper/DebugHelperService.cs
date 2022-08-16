using TMPro;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugHelperService : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugShowSceneId;
        [SerializeField] private TextMeshProUGUI debugShowPartCount;
        
        // [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void ShowSceneId(string sceneId)
        {
            string show = $"Scene id: [{sceneId}]";
            debugShowSceneId.text = show;
        }
        
        // [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void ShowPartCount(int part)
        {
            string show = $"Part: {part:00}";
            debugShowPartCount.text = show;
        }
    }
}