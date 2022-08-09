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
    }
}