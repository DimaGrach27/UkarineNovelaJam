using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using UnityEditor;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class SceneValidator : EditorWindow
    {
        [MenuItem("ReflectionOfAmber/Validate scenes")]
        public static void Validate()
        {
            ScreenSceneScriptableObject[] list = Resources.LoadAll<ScreenSceneScriptableObject>("Graphs");
            
            foreach (var screenScene in list)
            {
                
                int index = 0;
                foreach (var nextScene in screenScene.nextScenes)
                {
                    if (nextScene.Scene == null)
                    {
                        Debug.LogWarning($"[VALIDATOR] Scene: {screenScene.SceneKey} has missing scene in choose {index}");
                    }

                    if(nextScene.specialDependent.enable)
                    {
                        foreach (var special in nextScene.specialDependent.special)
                        {
                            if (special == null)
                            {
                                Debug.LogWarning($"[VALIDATOR] Scene: {screenScene.SceneKey} has missing special scene in choose {index}");
                            }
                            // switch (special.GetType())
                            // {
                            //     
                            //         break;
                            // }
                        }
                    }
                    
                    index++;
                }
            }
        }
    }
}