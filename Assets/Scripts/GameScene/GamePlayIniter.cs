using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ScreenPart;
using GameScene.ScreenText;
using GameScene.Services;
using UnityEngine;

namespace GameScene
{
    public class GamePlayIniter : MonoBehaviour
    {
        [SerializeField] private Transform uiCanvas;
        [SerializeField] private ScreenPartsService screenPartsService;

        //Services
        private CharacterService _characterService;
        private ScreenTextService _screenTextService;
        private BgService _bgService;

        //Handlers
        [SerializeField] private UiClickHandler uiClickHandler;

        private void Awake()
        {
            InitServices();
        }

        private void Start()
        {
            screenPartsService.
                SetServices(
                    _bgService,
                    _characterService,
                    _screenTextService,
                    uiClickHandler);
            
            screenPartsService.Init();
            
            FadeService.FadeService.FadeOut();
        }

        private void InitServices()
        {
            _characterService = new CharacterService(uiCanvas);
            _screenTextService = new ScreenTextService(uiCanvas);
            _bgService = new BgService(uiCanvas);
        }
    }
}