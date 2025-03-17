using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndSceneDemo
{
    public class EndSceneDemoInstaller : MonoInstaller
    {
        [SerializeField] 
        private EndSceneDemoService m_EndSceneDemoService;
        
        public override void InstallBindings()
        {
            Container.Bind<EndSceneDemoService>().FromInstance(m_EndSceneDemoService).AsSingle();
        }
    }
}