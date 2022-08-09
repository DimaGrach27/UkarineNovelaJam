using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ReflectionOfAmber.Scripts;
using ReflectionOfAmber.Scripts.GameScene;
using ReflectionOfAmber.Scripts.GameScene.NoteWindow;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindow
{
    public class NoteWindowUIView : MonoBehaviour
    {
        private const string KEY_TUTUOR_NOTE = "NOTE_TUTOR_KEY";
        public event Action<int> OnChoose;
        
        [SerializeField] private InfoDescription infoDescription;
        [SerializeField] private Button buttonReturn;
        [SerializeField] private Button buttonOpen;
        [SerializeField] private NoteButtonUiView[] buttonPrefab;

        private Coroutine _routine;

        private readonly Dictionary<KillerName, NoteButtonUiView> _killersMap = new();

        private bool IsTutorWasShow
        {
            get => PlayerPrefs.GetInt(KEY_TUTUOR_NOTE, 0) != 0;

            set => PlayerPrefs.SetInt(KEY_TUTUOR_NOTE, value ? 1 :0);
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
                // keyValue.Value.Visible = count > 0;
                // keyValue.Value.Visible = true;
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
            if(GameModel.GetStatus(StatusEnum.CHOOSE_WAS_PICK)) return;
            
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
            yield return new WaitForSeconds(duration * 2);

            if (!IsTutorWasShow)
            {
                List<string> texts = new()
                {
                    "Перед тобою записник Вільшанки, з яким вона не розстається з того моменту, " +
                    "як вступила на службу до поліції. Він зберігає багато спогадів, розкритих і не розкритих справ.",
                    
                    "Цього разу вона теж використовуватиме його за призначенням. " +
                    "Вільшанка буде заносити сюди підозрюваних та кількість зачіпок на кожну людину, " +
                    "які вона змогла знайти. Врешті решт ви зможете самі обрати, кого Вільшанка заарештує. " +
                    "Проте робіть цей вибір тільки коли будете упевнені. " +
                    "Будьте уважні, щоб іще один злочинець не залишився на волі через необачність поліції.",
                    
                    "Тому, коли будете упевнені хто злодій, натисніть на ім'я у списку."
                };
                
                InfoDescription infoDesc = Instantiate(infoDescription);
                infoDesc.SetInfoDescription(texts);

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