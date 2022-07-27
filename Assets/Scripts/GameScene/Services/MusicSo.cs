using GameScene.Services;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MusicSo", 
    menuName = "Frog Croaked Team/Create 'MusicSo'", 
    order = 0)]

public class MusicSo : ScriptableObject
{
    public MusicType type;
    public AudioClip clip;
}