using UnityEngine;

namespace GameScene.Services
{
    public class AudioSystemService : MonoBehaviour
    {
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundAudioSource;

        public static AudioSystemService Inst { get; private set; }
        
        private void Awake()
        {
            Inst = this;

            musicAudioSource.volume = SaveService.GetMusicVolume();
            soundAudioSource.volume = SaveService.GetAudioVolume();
        }

        public void StarPlayMusic(AudioClip audioClip)
        {
            musicAudioSource.clip = audioClip;
            musicAudioSource.Play();
        }
        
        public void PlayShotSound(AudioClip audioClip)
        {
            soundAudioSource.PlayOneShot(audioClip);
        }

        public void ChangeMusic(float value) => musicAudioSource.volume = value;
        public void ChangeAudio(float value) => soundAudioSource.volume = value;
    }
}