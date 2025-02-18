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
        [SerializeField] private NoteWindowInvestigationScreen noteWindowInvestigationScreen;
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
            
            Container.Bind<NoteWindowInvestigationScreen>()
                .FromInstance(noteWindowInvestigationScreen).AsSingle().NonLazy();
            Container.Bind<NoteWindowSaveScreen>()
                .FromInstance(noteWindowSaveScreen).AsSingle().NonLazy();
            Container.Bind<NoteWindowSettingsScreenView>()
                .FromInstance(noteWindowSettingsScreenView).AsSingle().NonLazy();
            Container.Bind<NoteWindowMainScreenView>()
                .FromInstance(noteWindowMainScreenView).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NoteWindowSettingsScreenService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowInvestigationScreenService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowSaveScreenService>().AsSingle().NonLazy();
        }
    }
}