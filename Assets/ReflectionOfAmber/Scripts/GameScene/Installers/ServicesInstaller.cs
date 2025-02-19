using ReflectionOfAmber.Scripts.DebugHelper;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.ChapterNotes;
using ReflectionOfAmber.Scripts.GameScene.Characters;
using ReflectionOfAmber.Scripts.GameScene.ChooseWindow;
using ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction;
using ReflectionOfAmber.Scripts.GameScene.GlobalVolume;
using ReflectionOfAmber.Scripts.GameScene.Health;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts.GameScene.ScreenText;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Installers
{
    public class ServicesInstaller : MonoInstaller<ServicesInstaller>
    {
        // [SerializeField] private DebugHelperService debugHelperService;
        [SerializeField] private GamePlayCanvas gamePlayCanvas;
        [SerializeField] private UiClickHandler uiClickHandler;
        [SerializeField] private GlobalVolumeService globalVolumeService;
        [SerializeField] private OpenEyeAnimation openEyeAnimation;
        [SerializeField] private ScreenPartNextDialogButton screenPartNextDialogButton;
        [SerializeField] private ChapterNotesView chapterNotesView;
        
        public override void InstallBindings()
        {
            GamePlayCanvasInstallers();
            // DebugInstallers();
            
            ServicesInstallers();
        }
        
        private void GamePlayCanvasInstallers()
        {
            Container.Bind<GamePlayCanvas>().FromInstance(gamePlayCanvas).AsSingle().NonLazy();
            Container.Bind<ScreenPartNextDialogButton>().FromInstance(screenPartNextDialogButton).AsSingle().NonLazy();
            Container.Bind<UiClickHandler>().FromInstance(uiClickHandler).AsSingle().NonLazy();
        }
        
        // private void DebugInstallers()
        // {
        //     Container.Bind<DebugHelperService>().FromInstance(debugHelperService).AsSingle().NonLazy();
        // }
        
        private void ServicesInstallers()
        {
            Container.Bind<GlobalVolumeService>().FromInstance(globalVolumeService).AsSingle().NonLazy();
            Container.Bind<OpenEyeAnimation>().FromInstance(openEyeAnimation).AsSingle().NonLazy();
            Container.Bind<CharacterService>().AsSingle().NonLazy();
            Container.Bind<ScreenTextService>().AsSingle().NonLazy();
            Container.Bind<BgService>().AsSingle().NonLazy();
            Container.Bind<ChooseWindowService>().AsSingle().NonLazy();
            Container.Bind<CameraActionService>().AsSingle().NonLazy();
            // Container.Bind<NoteService>().AsSingle().NonLazy();
            Container.Bind<HealthService>().AsSingle().NonLazy();
            
            Container.Bind<ActionScreenService>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<ScreenPartsService>().AsSingle().NonLazy();

            Container.Bind<ChapterNotesView>().FromInstance(chapterNotesView).AsSingle().NonLazy();
            Container.Bind<ChapterNotesService>().AsSingle().NonLazy();
        }
    }
}