using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class FinalBuildCleaner : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
#if GAME_FINAL
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.CompareTag("FinalExclude"))
                {
                    Object.DestroyImmediate(obj);
                }
            }
#endif
        }
    }
}