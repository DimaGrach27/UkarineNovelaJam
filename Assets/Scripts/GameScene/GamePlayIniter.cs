using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ChooseWindow;
using GameScene.ChooseWindow.CameraAction;
using GameScene.Health;
using GameScene.NoteWindow;
using GameScene.ScreenPart;
using GameScene.ScreenPart.ActionScreens;
using GameScene.ScreenText;
using GameScene.Services;
using UnityEngine;

namespace GameScene
{
    public class GamePlayIniter : MonoBehaviour
    {
        [SerializeField] private Transform uiCanvas;

        private ScreenPartsService _screenPartsService;

        //Services
        private CharacterService _characterService;
        private ScreenTextService _screenTextService;
        private BgService _bgService;
        private ChooseWindowService _chooseWindowService;
        private CameraActionService _cameraActionService;
        private ActionScreenService _actionScreenService;
        private NoteService _noteService;
        private HealthService _healthService;

        //Handlers
        [SerializeField] private UiClickHandler uiClickHandler;
        
        private void Awake()
        {
            InitServices();
        }

        private void Start()
        {
            _screenPartsService = new ScreenPartsService(
                _bgService,
                _characterService,
                _screenTextService,
                uiClickHandler,
                _chooseWindowService,
                _cameraActionService,
                _actionScreenService
                );
            
            _screenPartsService.Init();
        }

        private void InitServices()
        {
            _characterService = new CharacterService(uiCanvas);
            _screenTextService = new ScreenTextService(uiCanvas, uiClickHandler);
            _bgService = new BgService(uiCanvas);
            _chooseWindowService = new ChooseWindowService(uiCanvas);
            _cameraActionService = new CameraActionService(uiCanvas);
            _healthService = new HealthService(uiCanvas);
            _noteService = new NoteService(uiCanvas);

            _actionScreenService = new ActionScreenService(_healthService, _cameraActionService, uiCanvas);
        }
    }
}