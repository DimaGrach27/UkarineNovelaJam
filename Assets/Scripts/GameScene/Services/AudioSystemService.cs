using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Services
{
    public class AudioSystemService : MonoBehaviour
    {
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundAudioSource;


        private Dictionary<MusicType, AudioClip> _audioClipsMap = new();
        public static AudioSystemService Inst { get; private set; }

        private Queue<AudioClip> _audioClips = new();

        private AudioClip _audioClip;

        private Coroutine _coroutine;

        private void Awake()
        {
            if(Inst == null)
                Inst = this;

            musicAudioSource.volume = SaveService.GetMusicVolume();
            soundAudioSource.volume = SaveService.GetAudioVolume();

            foreach (var musicSo in Resources.LoadAll<MusicSo>("Music"))
            {
                _audioClipsMap.Add(musicSo.type, musicSo.clip);
            }

            _coroutine = StartCoroutine(PlayRoutine());
        }

        public void StopMusic()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _audioClip = null;
            musicAudioSource.Stop();
        }
        
        public void StarPlayMusic(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _audioClips.Clear();
            
            _audioClip = _audioClipsMap[type];
            
            musicAudioSource.clip = _audioClipsMap[type];
            
            musicAudioSource.Stop();
            musicAudioSource.Play();
            
            _coroutine = StartCoroutine(PlayRoutine());
        }
        
        public void StarPlayMusic(AudioClip audioClip)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _audioClips.Clear();
            
            _audioClip = audioClip;
            
            musicAudioSource.clip = audioClip;
            
            musicAudioSource.Stop();
            musicAudioSource.Play();
            
            _coroutine = StartCoroutine(PlayRoutine());
        }
        
        public void PlayShotSound(AudioClip audioClip)
        {
            soundAudioSource.PlayOneShot(audioClip);
        }
        
        public void PlayShotSound(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            soundAudioSource.PlayOneShot(_audioClipsMap[type]);
        }

        public void ChangeMusic(float value) => musicAudioSource.volume = value;
        public void ChangeAudio(float value) => soundAudioSource.volume = value;

        public AudioSource AudioSourceMusic => musicAudioSource;


        IEnumerator PlayRoutine()
        {
            while (true)
            {
                if (!musicAudioSource.isPlaying)
                {
                    if (_audioClips.TryDequeue(out AudioClip result))
                    {
                        _audioClip = result;
                    }

                    if (_audioClip != null)
                    {
                        musicAudioSource.clip = _audioClip;
                    
                        musicAudioSource.Play();
                    }
                }
                
                yield return null;
            }
        }
        
        public void AddQueueClip(AudioClip audioClip)
        {
            _audioClips.Enqueue(audioClip);
        }
        
        public void AddQueueClip(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            
            _audioClips.Enqueue(_audioClipsMap[type]);
        }
    }

    public enum MusicType
    {
        NONE = 0,
        NEBO = 1,
        EMBIENT_SLOW = 2,
        EMBIENT_FAST = 3,
        RADIO_CHANGE = 4,
        PREPARE_CRASH = 5,
        CRASH = 6,
        PHONE_BEEP = 7,
    }
    
}