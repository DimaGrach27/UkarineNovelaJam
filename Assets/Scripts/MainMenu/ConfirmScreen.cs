using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ConfirmScreen : MonoBehaviour
    {
        public static ConfirmScreen Ins { get; private set; }
        
        [SerializeField] private TextMeshProUGUI textDescription;
        
        [SerializeField] private Button confirm;
        [SerializeField] private Button notConfirm;

        private CanvasGroup _canvasGroup;
        private Action<bool> _currentAction;

        private Tween _tween;

        private void Awake()
        {
            if(Ins == null)
                Ins = this;
            
            confirm.onClick.AddListener(Confirm);
            notConfirm.onClick.AddListener(NotConfirm);
            
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Check(Action<bool> onSelectAction, string description)
        {
            if (_tween != null)
                DOTween.Kill(_tween);
            
            _canvasGroup.blocksRaycasts = true;
            _tween = _canvasGroup.DOFade(1.0f, 0.5f);
            
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
            if (_tween != null)
                DOTween.Kill(_tween);

            _canvasGroup.blocksRaycasts = false;
            _tween = _canvasGroup.DOFade(0.0f, 0.5f);
            
            _currentAction = null;
        }
    }
}