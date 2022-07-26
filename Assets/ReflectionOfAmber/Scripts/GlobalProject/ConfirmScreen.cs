﻿using System;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class ConfirmScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDescription;
        
        [SerializeField] private Button confirm;
        [SerializeField] private Button notConfirm;

        private CanvasGroup _canvasGroup;
        private Action<bool> _currentAction;

        private Tween _tween;

        private void Awake()
        {
            confirm.onClick.AddListener(Confirm);
            notConfirm.onClick.AddListener(NotConfirm);
            
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Check(Action<bool> onSelectAction, TranslatorKeys translatorKey)
        {
            if (_tween != null)
                DOTween.Kill(_tween);
            
            _canvasGroup.blocksRaycasts = true;
            _tween = _canvasGroup.DOFade(1.0f, 0.5f);
            
            textDescription.text = TranslatorParser.GetText(translatorKey);
            
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