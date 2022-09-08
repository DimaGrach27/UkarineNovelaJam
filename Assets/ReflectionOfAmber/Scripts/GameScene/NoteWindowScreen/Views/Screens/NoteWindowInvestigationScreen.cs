using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindow;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens
{
    public class NoteWindowInvestigationScreen : NoteWindowScreenBase
    {
        public override NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.INVESTIGATION_SCREEN;
        
        public event Action<int> OnChoose;
        
        [SerializeField] private NoteButtonUiView[] buttonPrefab;

        private readonly Dictionary<KillerName, NoteButtonUiView> _killersMap = new();

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