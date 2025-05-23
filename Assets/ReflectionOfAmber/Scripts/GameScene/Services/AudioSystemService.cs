using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Services
{
    public class AudioSystemService : MonoBehaviour
    {
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundAudioSource;
        [SerializeField] private AudioSource soundAudioLooperSource;

        private Dictionary<MusicType, MusicSo> _audioClipsMap = new();
        
        private readonly Queue<AudioClip> _audioClips = new();
        private readonly Queue<AudioClip> _audioClipsLooper = new();

        private AudioClip _audioClipLoop;

        private Coroutine _coroutine;
        private Coroutine _coroutineLoop;

        private bool m_appHidden;

        public AudioSource AudioSourceMusic => musicAudioSource;
        public AudioSource SoundAudioLooperSource => soundAudioLooperSource;

        [Inject]
        public void Construct(GameResourcesService gameResourcesService)
        {
            _audioClipsMap = gameResourcesService.AudioClipsMap;
        }

        private void Awake()
        {
            musicAudioSource.volume = SaveService.MusicVolume;
            soundAudioSource.volume = SaveService.AudioVolume;
            soundAudioLooperSource.volume = SaveService.AudioVolume;

            m_appHidden = false;
            
            _coroutine = StartCoroutine(PlayRoutineLoop());
            _coroutineLoop = StartCoroutine(PlayRoutineLooper());
        }

        public void StopAllMusic()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            if (_coroutineLoop != null)
                StopCoroutine(_coroutineLoop);
            
            _audioClipLoop = null;
            musicAudioSource.Stop();
            soundAudioLooperSource.Stop();
        }
        
        public void StopSoundMusic()
        {
            if (_coroutineLoop != null)
                StopCoroutine(_coroutineLoop);

            LoopForLopper = false;
            _audioClipsLooper.Clear();
            soundAudioLooperSource.Stop();
        }
        
        public void StarPlayMusicOnLoop(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _audioClips.Clear();
            
            _audioClipLoop = _audioClipsMap[type].clip;
            
            musicAudioSource.clip = _audioClipsMap[type].clip;
            
            musicAudioSource.Stop();
            musicAudioSource.Play();
            
            _coroutine = StartCoroutine(PlayRoutineLoop());
        }

        public AudioClip GetClip(MusicType type)
        {
            if (_audioClipsMap.ContainsKey(type))
                return _audioClipsMap[type].clip;

            return null;
        }
        public void StarPlayMusicOnLooper(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            
            if (_coroutineLoop != null)
                StopCoroutine(_coroutineLoop);
            
            _audioClipsLooper.Clear();

            soundAudioLooperSource.clip = _audioClipsMap[type].clip;
            
            soundAudioLooperSource.Stop();
            soundAudioLooperSource.Play();
            
            _coroutineLoop = StartCoroutine(PlayRoutineLooper());
        }

        public bool LoopForLopper
        {
            set => soundAudioLooperSource.loop = value;
        }
        
        public void PlayShotSound(AudioClip audioClip)
        {
            soundAudioSource.PlayOneShot(audioClip);
        }
        
        public void PlayShotSound(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            soundAudioSource.PlayOneShot(_audioClipsMap[type].clip);
        }

        public void ChangeMusic(float value) => musicAudioSource.volume = value;
        public void ChangeAudio(float value)
        { 
            soundAudioSource.volume = value;
            soundAudioLooperSource.volume = value;
        }

        
        public void AddQueueClipToLoop(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            
            _audioClips.Enqueue(_audioClipsMap[type].clip);
        }
        
        public void AddQueueClipToSound(MusicType type)
        {
            if(!_audioClipsMap.ContainsKey(type)) return;
            
            _audioClipsLooper.Enqueue(_audioClipsMap[type].clip);
        }
        
        IEnumerator PlayRoutineLoop()
        {
            while (true)
            {
                if (!musicAudioSource.isPlaying && !m_appHidden)
                {
                    if (_audioClips.TryDequeue(out AudioClip result))
                    {
                        _audioClipLoop = result;
                    }

                    if (_audioClipLoop != null)
                    {
                        musicAudioSource.clip = _audioClipLoop;
                    
                        musicAudioSource.Play();
                    }
                }
                
                yield return null;
            }
        }
        
        IEnumerator PlayRoutineLooper()
        {
            while (true)
            {
                if(!soundAudioLooperSource.loop)
                {
                    if (!soundAudioLooperSource.isPlaying)
                    {
                        if (_audioClipsLooper.TryDequeue(out AudioClip result))
                        {
                            soundAudioLooperSource.clip = result;

                            soundAudioLooperSource.Play();
                        }
                    }
                }
                
                yield return null;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            m_appHidden = pause;
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
        FALLEN_TREE = 8,
        WATER_SLAP = 9,
        METAL_DRAG = 10,
        DOG_BARK = 11,
        RADIO_COPS = 12,
        PUNCH = 13,
        HEART_BEEP = 14,
        CREAKING_DOOR = 15,
        PISTOL_SHOT = 16,
        PHOTO_CLICK = 17,
    }
    
}