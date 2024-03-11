using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HDH.Audio.Confgis;
using HDH.Audio.Music;
using HDH.Audio.Player3D;
using UnityEngine;
using UnityEngine.Audio;

namespace HDH.Audio
{
    public class AudioService
    {
        public AudioPlayer Player => _player;
        public AudioPlayer3D Player3D => _player3d;
        public Transform Transform => _agent.transform;
        public MusicPlayer MusicPlayer => _musicPlayer;
        
        private readonly AudioMixer _audioMixer;
        private readonly AudioServiceConfig _config;
        private readonly AudioServiceAgent _agent;
        private readonly Dictionary<string, CancellationTokenSource> _fadeSources;
        private AudioPlayer _player;
        private AudioPlayer3D _player3d;
        private MusicPlayer _musicPlayer;

        public AudioService(AudioServiceConfig config)
        {
            _config = config;
            _agent = new GameObject("Audio Service").AddComponent<AudioServiceAgent>();
            _audioMixer = _config.Mixer;
            _fadeSources = new Dictionary<string, CancellationTokenSource>();
            InstantiatePlayers();
            InitializeMixerGroups().Forget();
            _agent.Initialize(this);
        }

        public float GetGroupVolume(string paramId) => 
            PlayerPrefs.GetFloat(paramId);

        public void SetGroupVolumeByName(string groupName, float normalizedVolume, float fadeDuration = 0)
        {
            SetGroupVolumeWithoutSaving(groupName, normalizedVolume, fadeDuration).Forget();
            PlayerPrefs.SetFloat(groupName, normalizedVolume);
        }

        public bool IsGroupActive(string paramId) => 
            GetGroupVolume(paramId) > 0;

        public void SetActiveGroup(string paramId, bool isActive) => 
            SetGroupVolumeByName(paramId, isActive ? 1 : 0);

        public void SetPauseGroup(string paramId, bool isPaused)
        {
            SetGroupVolumeWithoutSaving(paramId, isPaused ? 0 : GetGroupVolume(paramId), 0).Forget();
        }

        private void InstantiatePlayers()
        {
            _player = new AudioPlayer(this, _config.SourcesLimit2D);
            _player3d = new AudioPlayer3D(this, _config);
            _musicPlayer = new MusicPlayer(this, _config);
        }

        private async UniTaskVoid InitializeMixerGroups()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            foreach (string mixerGroup in _config.MixerVolumeParameters)
            {
                float volume = 1;
                if (PlayerPrefs.HasKey(mixerGroup))
                {
                    volume = PlayerPrefs.GetFloat(mixerGroup);
                }
                else
                {
                    PlayerPrefs.SetFloat(mixerGroup, 1);
                }

                SetGroupVolumeByName(mixerGroup, volume);
            }
        }

        private async UniTaskVoid SetGroupVolumeWithoutSaving(string groupName, float normalizedVolume, float fadeDuration)
        {
            if (_fadeSources.TryGetValue(groupName, out var ctSource))
            {
                if (ctSource?.IsCancellationRequested == false)
                {
                    ctSource.Cancel();
                }
            }
            else
            {
                _fadeSources.Add(groupName, null);
            }

            if (fadeDuration == 0)
            {
                _audioMixer.SetFloat(groupName, GetVolume(normalizedVolume));
                return;
            }

            CancellationTokenSource cts = new CancellationTokenSource();
            _fadeSources[groupName] = cts;
            if (fadeDuration <= 0)
                throw new ArgumentException();

            float currentDuration = 0;
            float startVolume = GetGroupVolume(groupName);
            
            while (cts.IsCancellationRequested == false && currentDuration < fadeDuration)
            {
                float volumeValue =
                    GetVolume(Mathf.Lerp(startVolume, normalizedVolume, currentDuration / fadeDuration));
                _audioMixer.SetFloat(groupName, volumeValue);
                currentDuration += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cts.Token);
            }
            
            _audioMixer.SetFloat(groupName, GetVolume(normalizedVolume));
        }

        private float GetVolume(float value) => 
            Mathf.Lerp(-80, 0, value);
        
        public enum Priority { Default, High }
    }
}