using ReflectionOfAmber.Scripts.Input;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TesteSceneInstaller : MonoInstaller<TesteSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
        }
    }
}