using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Handlers;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Installers
{
    public class NoteInstaller : MonoInstaller<NoteInstaller>
    {
        [Header("Note")]
        [SerializeField] private NoteWindowScreenPopup noteWindowScreen;
// #if !GAME_DEMO
        [SerializeField] private NoteWindowInvestigationScreen noteWindowInvestigationScreen;
// #endif
        [SerializeField] private NoteWindowSaveScreen noteWindowSaveScreen;
        [SerializeField] private NoteWindowSettingsScreenView noteWindowSettingsScreenView;
        [SerializeField] private NoteWindowMainScreenView noteWindowMainScreenView;
        
        public override void InstallBindings()
        {
            NoteInstallers();
        }
        
        private void NoteInstallers()
        {
            Container.Bind<NoteWindowScreenPopup>().FromInstance(noteWindowScreen).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NoteWindowScreenPopupService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowScreenChangeHandler>().AsSingle().NonLazy();
            
// #if !GAME_DEMO
            Container.BindInterfacesAndSelfTo<NoteWindowInvestigationScreen>()
                .FromInstance(noteWindowInvestigationScreen).AsSingle().NonLazy();
// #endif

            Container.BindInterfacesAndSelfTo<NoteWindowSaveScreen>()
                .FromInstance(noteWindowSaveScreen).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowSettingsScreenView>()
                .FromInstance(noteWindowSettingsScreenView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowMainScreenView>()
                .FromInstance(noteWindowMainScreenView).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NoteWindowSettingsScreenService>().AsSingle().NonLazy();
// #if !GAME_DEMO
            Container.BindInterfacesAndSelfTo<NoteWindowInvestigationScreenService>().AsSingle().NonLazy();
// #endif
            Container.BindInterfacesAndSelfTo<NoteWindowSaveScreenService>().AsSingle().NonLazy();
        }
    }
}