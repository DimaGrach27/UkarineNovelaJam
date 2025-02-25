using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindow;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens
{
    public class NoteWindowInvestigationScreen : NoteWindowScreenBase
    {
        public override NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.INVESTIGATION_SCREEN;
        
        public event Action<int> OnChoose;
        
        [SerializeField] private NoteButtonUiView[] buttonPrefab;

        private readonly Dictionary<KillerName, NoteButtonUiView> _killersMap = new();
        
        private const string KEY_TUTUOR_NOTE = "NOTE_TUTOR_KEY";
        private InfoDescription.Factory m_infoDescriptionFactory;
        
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
                buttonPrefab[i].InitButton(i);
            }
        }

        public override void Open()
        {
            base.Open();
            InitNote();
            
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

        private void NoteButtonUiViewOnOnChoose(int index)
        {
            OnChoose?.Invoke(index);
        }
        
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
    }
}