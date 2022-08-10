using ReflectionOfAmber.Scripts.GameScene;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class GlobalButton : MonoBehaviour
    {
        [SerializeField] private CallKeyType keyType;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            GlobalEvent.CallType(keyType);
        }
    }
}