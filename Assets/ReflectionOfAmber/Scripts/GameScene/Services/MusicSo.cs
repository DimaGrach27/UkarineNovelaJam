using System;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.Services
{
    [CreateAssetMenu(
        fileName = "MusicSo", 
        menuName = "Frog Croaked Team/Create 'MusicSo'", 
        order = 0)]

    public class MusicSo : ScriptableObject
    {
        public MusicType type;
        public AudioClip clip;
        public OverridenSettings overridenSettings;
    }
    
    public struct MusicSetting
    {
        public AudioClip clip;
        public OverridenSettings overridenSettings;
    }
    
    [Serializable]
    public struct OverridenSettings
    {
        public bool enable;
        public float volume;
    }
}