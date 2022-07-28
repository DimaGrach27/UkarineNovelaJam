using System;
using GameScene.ScreenPart;
using MainMenu;
using UnityEngine;

namespace GameScene.NoteWindow
{
    public class NoteService
    {
        private readonly ScreenPartsService _screenPartsService;
        private readonly string[] _nextScenes;
        
        private int _selectIndex = -1;

        public NoteService(Transform ui,ScreenPartsService screenPartsService)
        {
            _screenPartsService = screenPartsService;
            
            _nextScenes = new string[3];

            _nextScenes[0] = "scene_3_164_3";
            _nextScenes[1] = "scene_3_164_1";
            _nextScenes[2] = "scene_3_164_2";
            
            var noteWindowUIView = ui.GetComponentInChildren<NoteWindowUIView>();
            noteWindowUIView.OnChoose += OnChooseClick;
        }

        private void OnChooseClick(int index)
        {
            _selectIndex = index;
            string description = "Ви впевнені, що саме цей підозрюваний являється злодієм? " +
                                 "Помилка призведе до необачних дій...";
            
            ConfirmScreen.Ins.Check(ConfirmAction, description);
        }

        private void ConfirmAction(bool isConfirm)
        {
            if (isConfirm)
            {
                _screenPartsService.ShowNextScene(_nextScenes[_selectIndex]);
            }

            _selectIndex = -1;
        }
    }
}