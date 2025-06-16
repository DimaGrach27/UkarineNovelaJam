using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class UIBindingHint : MonoBehaviour
    {
        [SerializeField] 
        private InputActionID m_InputActionID;
        
        [SerializeField] 
        private Image m_icon;

        private void Start()
        {
            UpdateHint();
        }

        public void OnEnable()
        {
            UpdateHint();
        }

        public void UpdateHint()
        {
            m_icon.sprite = PlayerInputHandler.Instance.GetIconHint(m_InputActionID);
        }
    }
}