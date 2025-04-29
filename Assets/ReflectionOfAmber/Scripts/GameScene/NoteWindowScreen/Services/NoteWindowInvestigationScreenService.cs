using System;
#if ANALYTIC_ENABLED
using ReflectionOfAmber.Scripts.Analytic;
using ReflectionOfAmber.Scripts.Analytic.Events;
#endif
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowInvestigationScreenService : IDisposable
    {
        private readonly NoteWindowInvestigationScreen m_NoteWindowInvestigationScreen;
        private readonly ScreenPartsServiceFacade m_ScreenPartsServiceFacade;
        private readonly ConfirmScreen m_ConfirmScreen;
#if ANALYTIC_ENABLED
        private readonly AnalyticService m_AnalyticService;
#endif
        private readonly string[] m_NextScenes;
        
        private int m_SelectIndex = -1;

        [Inject]
        public NoteWindowInvestigationScreenService(
            NoteWindowInvestigationScreen noteWindowInvestigationScreen, 
            ScreenPartsServiceFacade screenPartsServiceFacade,
            ConfirmScreen confirmScreen
#if ANALYTIC_ENABLED
            ,AnalyticService analyticService
#endif
            )
        {
            m_ScreenPartsServiceFacade = screenPartsServiceFacade;
            m_ConfirmScreen = confirmScreen;
#if ANALYTIC_ENABLED

            m_AnalyticService = analyticService;
#endif
            m_NoteWindowInvestigationScreen = noteWindowInvestigationScreen;
            
            m_NextScenes = new string[3];
#if !GAME_DEMO
            m_NextScenes[0] = "scene_3_164_3";
            m_NextScenes[1] = "scene_3_164_1";
            m_NextScenes[2] = "scene_3_164_2";
            
            m_NoteWindowInvestigationScreen.OnChoose += OnChooseClick;
#endif
        }
        

        private void OnChooseClick(int index)
        {
            m_SelectIndex = index;
            m_ConfirmScreen.Check(ConfirmAction, TranslatorKeys.CONFIRM_CHOOSE);
        }

        private void ConfirmAction(bool isConfirm)
        {
            if (isConfirm)
            {
#if ANALYTIC_ENABLED
                m_AnalyticService.ReportEvent(new KillerChosenAnalyticEvent((KillerName)m_SelectIndex));
#endif
                SaveService.SetStatusValue(StatusEnum.CHOOSE_WAS_PICK, true);
                m_ScreenPartsServiceFacade.PlayNextScene(m_NextScenes[m_SelectIndex]);
            }

            m_SelectIndex = -1;
        }

        public void Dispose()
        {
            m_NoteWindowInvestigationScreen.OnChoose -= OnChooseClick;
        }
    }
}