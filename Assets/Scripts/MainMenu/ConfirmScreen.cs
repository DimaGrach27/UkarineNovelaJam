using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ConfirmScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDescription;
        
        [SerializeField] private Button confirm;
        [SerializeField] private Button notConfirm;

        private Action<bool> _currentAction;

        private void Awake()
        {
            confirm.onClick.AddListener(Confirm);
            notConfirm.onClick.AddListener(NotConfirm);
        }

        public void Check(Action<bool> onSelectAction, string description)
        {
            gameObject.SetActive(true);
            
            textDescription.text = description;
            
            _currentAction = onSelectAction;
        }

        private void Confirm()
        {
            _currentAction?.Invoke(true);
            Close();
        }
        
        private void NotConfirm()
        {
            _currentAction?.Invoke(false);
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
            _currentAction = null;
        }
    }
}