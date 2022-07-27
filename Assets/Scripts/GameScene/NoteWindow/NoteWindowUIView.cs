using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.NoteWindow
{
    public class NoteWindowUIView : MonoBehaviour
    {
        public event Action<int> OnChoose;
        
        [SerializeField] private Button buttonReturn;
        [SerializeField] private Button buttonOpen;
        [SerializeField] private NoteButtonUiView[] buttonPrefab;

        private Coroutine _routine;

        private readonly Dictionary<KillerName, NoteButtonUiView> _killersMap = new();

        private void Awake()
        {
            for (int i = 0; i < buttonPrefab.Length; i++)
            {
                buttonPrefab[i].OnChoose += NoteButtonUiViewOnOnChoose;

                KillerName killerName = (KillerName)(i + 1);
                _killersMap.Add(killerName, buttonPrefab[i]);
                
                buttonPrefab[i].Visible = false;
                buttonPrefab[i].InitButton(i + 1);;
            }

            buttonReturn.onClick.AddListener(Close);
            buttonOpen.onClick.AddListener(Open);
            
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.alpha = 0.0f;
        }

        private void NoteButtonUiViewOnOnChoose(int index)
        {
            OnChoose?.Invoke(index);
        }

        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;
        

        private void InitNote()
        {
            foreach (var keyValue in _killersMap)
            {
                int count = GameModel.GetInt(keyValue.Key);
                keyValue.Value.UpdateButton(count);
                keyValue.Value.Visible = count > 0;
            }
        }

        private void Close()
        {
            if(_routine != null)
                CoroutineHelper.Inst.StopCoroutine(_routine);

            _routine = CoroutineHelper.Inst.StartCoroutine(FadeOutWindow());
        }
        private void Open()
        {
            if(_routine != null)
                CoroutineHelper.Inst.StopCoroutine(_routine);

            InitNote();
            _routine = CoroutineHelper.Inst.StartCoroutine(FadeInWindow());
        }
        
        private IEnumerator FadeInWindow()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            CanvasGroup.DOFade(1.0f, duration);
            yield return null;
        }
        
        private IEnumerator FadeOutWindow()
        {
            CanvasGroup.interactable = false;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            CanvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            CanvasGroup.blocksRaycasts = false;
        }
    }
}