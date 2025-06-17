using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class UIBindingHint : MonoBehaviour, IDeviceChangeListener
    {
        [SerializeField] 
        private InputActionID m_InputActionID;
        
        [SerializeField] 
        private Image m_icon;

        private void Start()
        {
            PlayerInputHandler.Instance.Subscribe(this);
            UpdateHint();
        }

        public void OnEnable()
        {
            UpdateHint();
        }

        private void OnDestroy()
        {
            PlayerInputHandler.Instance.Unsubscribe(this);
        }

        private void UpdateHint()
        {
            m_icon.sprite = PlayerInputHandler.Instance.GetIconHint(m_InputActionID);
        }

        public void OnDeviceChangedHandler()
        {
            UpdateHint();
        }
    }
}