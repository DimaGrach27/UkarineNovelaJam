using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndScene
{
    public class EndSceneInstaller : MonoInstaller
    {
        [SerializeField] private  EndSceneService m_endSceneService;
        
        public override void InstallBindings()
        {
            Container.Bind<EndSceneService>().FromInstance(m_endSceneService).AsSingle();
        }
    }
}