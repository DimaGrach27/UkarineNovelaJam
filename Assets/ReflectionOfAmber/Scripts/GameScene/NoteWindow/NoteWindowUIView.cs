using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindow
{
    public class NoteWindowUIView : MonoBehaviour
    {
        private const string KEY_TUTUOR_NOTE = "NOTE_TUTOR_KEY";
        public event Action<int> OnChoose;
        
        [SerializeField] private Button buttonReturn;
        [SerializeField] private NoteButtonUiView[] buttonPrefab;

        private Coroutine _routine;
        private InfoDescription.Factory m_infoDescriptionFactory;

        private readonly Dictionary<KillerName, NoteButtonUiView> _killersMap = new();

        private bool IsTutorWasShow
        {
            get => PlayerPrefs.GetInt(KEY_TUTUOR_NOTE, 0) != 0;

            set => PlayerPrefs.SetInt(KEY_TUTUOR_NOTE, value ? 1 :0);
        }

        [Inject]
        public void Construct(InfoDescription.Factory infoDescriptionFactory)
        {
            m_infoDescriptionFactory = infoDescriptionFactory;
        }

        private void Awake()
        {
            for (int i = 0; i < buttonPrefab.Length; i++)
            {
                buttonPrefab[i].OnChoose += NoteButtonUiViewOnOnChoose;

                KillerName killerName = (KillerName)(i + 1);
                _killersMap.Add(killerName, buttonPrefab[i]);
                
                buttonPrefab[i].Visible = false;
                buttonPrefab[i].InitButton(i);;
            }

            buttonReturn.onClick.AddListener(Close);
            
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
                int count = SaveService.GetIntValue(keyValue.Key);
                keyValue.Value.UpdateButton(count);

                switch (keyValue.Key)
                {
                    case KillerName.ILONA_VOR:
                        keyValue.Value.Visible = SaveService.GetStatusValue(StatusEnum.ILONA_HAVE_SHOW);
                        break;
                    
                    case KillerName.OLEKSIY_VOR:
                        keyValue.Value.Visible = SaveService.GetStatusValue(StatusEnum.OLEKSII_HAVE_SHOW);
                        break;
                    
                    case KillerName.ZAHARES_VOR:
                        keyValue.Value.Visible = SaveService.GetStatusValue(StatusEnum.ZAHARES_HAVE_SHOW);
                        break;
                }
            }
        }

        private void Close()
        {
            if(_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(FadeOutWindow());
        }
        public void Open(bool canExit = true)
        {
            if(SaveService.GetStatusValue(StatusEnum.CHOOSE_WAS_PICK)) return;
            
            if(_routine != null)
                StopCoroutine(_routine);

            InitNote();
            _routine = StartCoroutine(FadeInWindow());
            buttonReturn.gameObject.SetActive(canExit);
        }

        private IEnumerator FadeInWindow()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            CanvasGroup.DOFade(1.0f, duration);
            yield return new WaitForSeconds(duration * 2);

            if (!IsTutorWasShow)
            {
                TranslatorKeys[] texts = 
                {
                    TranslatorKeys.TEXT_INFO_NOTE_PART_1,
                    TranslatorKeys.TEXT_INFO_NOTE_PART_2,
                    TranslatorKeys.TEXT_INFO_NOTE_PART_3,
                };

                
                InfoDescription infoDesc = m_infoDescriptionFactory.Create();
                infoDesc.SetInfoDescription(texts, false);

                IsTutorWasShow = true;
            }
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