using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public class GameResourcesService : IInit
    {
        private static readonly Dictionary<string, ScreenSceneScriptableObject> ScreenScenesMap = new();
        private static readonly Dictionary<CharacterName, CharacterNameScriptableObject> CharacterNameMap = new();
        private static readonly Dictionary<BgEnum, BgScriptableObject> BgMap = new();

        public event Action OnReady;

        private const string BG_ASSETS_GROUP = "background";
        private const string CHARACTER_ASSETS_GROUP = "character";
        private const string MUSIC_ASSETS_GROUP = "music";

        private readonly string[] SCENES_ASSETS_GROUP =
        {
            "prologue",
#if !GAME_DEMO
            "main_story"
#endif
        };

        private const int LOAD_GROUP_COUNT = 3;
        private int m_CurrentLoadCount = 0;

        public void Init()
        {
            LoadAllAssetsFromFolder();
        }

        private void LoadAllAssetsFromFolder()
        {
            m_CurrentLoadCount = LOAD_GROUP_COUNT;
            Debug.Log("Start load");
            Addressables
                    .LoadAssetsAsync<ScreenSceneScriptableObject>(SCENES_ASSETS_GROUP, OnAssetLoaded,
                        Addressables.MergeMode.Union).Completed +=
                _ =>
                {
                    m_CurrentLoadCount--;
                    OnAllAssetsLoaded();
                };

            Addressables.LoadAssetsAsync<BgScriptableObject>(BG_ASSETS_GROUP, OnAssetLoaded).Completed +=
                _ =>
                {
                    m_CurrentLoadCount--;
                    OnAllAssetsLoaded();
                };
            Addressables.LoadAssetsAsync<CharacterNameScriptableObject>(CHARACTER_ASSETS_GROUP, OnAssetLoaded)
                    .Completed +=
                _ =>
                {
                    m_CurrentLoadCount--;
                    OnAllAssetsLoaded();
                };
        }

        private void OnAssetLoaded(ScreenSceneScriptableObject handle)
        {
            Debug.Log($"KEY: {handle.SceneKey}");

            ScreenScenesMap.Add(handle.SceneKey, handle);
        }

        private void OnAssetLoaded(CharacterNameScriptableObject handle)
        {
            Debug.Log($"KEY: {handle.characterName}");

            CharacterNameMap.Add(handle.characterNameType, handle);
        }

        private void OnAssetLoaded(BgScriptableObject handle)
        {
            Debug.Log($"KEY: {handle.Bg}");

            BgMap.Add(handle.Bg, handle);
        }

        private void OnAllAssetsLoaded()
        {
            if (m_CurrentLoadCount > 0)
            {
                return;
            }
            Debug.Log("End load");

            OnReady?.Invoke();
        }
    }
}