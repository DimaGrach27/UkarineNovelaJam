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
        
        public override void InstallBindings()
        {
            NoteInstallers();
        }
        
        private void NoteInstallers()
        {
            Container.Bind<NoteWindowScreenPopup>().FromInstance(noteWindowScreen).AsSingle().NonLazy();
            Container.Bind<NoteWindowScreenPopupService>().AsSingle().NonLazy();
            Container.Bind<NoteWindowScreenChangeHandler>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NoteWindowInvestigationScreen>()
                .FromInstance(noteWindowInvestigationScreen).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowSaveScreen>()
                .FromInstance(noteWindowSaveScreen).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NoteWindowSettingsScreenView>()
                .FromInstance(noteWindowSettingsScreenView).AsSingle().NonLazy();
            
            Container.Bind<NoteWindowSettingsScreenService>().AsSingle().NonLazy();
            Container.Bind<NoteWindowInvestigationScreenService>().AsSingle().NonLazy();
            Container.Bind<NoteWindowSaveScreenService>().AsSingle().NonLazy();
        }
    }
}