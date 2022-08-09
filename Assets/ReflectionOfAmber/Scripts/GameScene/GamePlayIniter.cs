using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.Characters;
using ReflectionOfAmber.Scripts.GameScene.ChooseWindow;
using ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction;
using ReflectionOfAmber.Scripts.GameScene.Health;
using ReflectionOfAmber.Scripts.GameScene.NoteWindow;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts.GameScene.ScreenText;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene
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
        
        private void InitServices()
        {
            _screenPartsService = new ScreenPartsService();
            
            _characterService = new CharacterService(uiCanvas);
            _screenTextService = new ScreenTextService(uiCanvas, uiClickHandler);
            _bgService = new BgService(uiCanvas);
            _chooseWindowService = new ChooseWindowService(uiCanvas);
            _cameraActionService = new CameraActionService(uiCanvas);
            _healthService = new HealthService(uiCanvas);
            
            _actionScreenService = new ActionScreenService(
                _healthService, 
                _cameraActionService, 
                _screenPartsService, 
                uiCanvas);
            
            _noteService = new NoteService(uiCanvas, _screenPartsService);
        }
        
        private void Start()
        {
            _screenPartsService.InitServices(
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
    }
}