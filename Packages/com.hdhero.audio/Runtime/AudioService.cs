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
        private AudioPlayer _player;
        private AudioPlayer3D _player3d;
        private MusicPlayer _musicPlayer;

        public AudioService(AudioServiceConfig config)
        {
            _config = config;
            _agent = new GameObject("Audio Service").AddComponent<AudioServiceAgent>();
            _audioMixer = _config.Mixer;
            InstantiatePlayers();
            InitializeMixerGroups().Forget();
            _agent.Initialize(this);
        }

        public float GetGroupVolume(string paramId) => 
            PlayerPrefs.GetFloat(paramId);

        public void SetGroupVolumeByName(string groupName, float normalizedVolume)
        {
            SetGroupVolumeWithoutSaving(groupName, normalizedVolume);
            PlayerPrefs.SetFloat(groupName, normalizedVolume);
        }

        public bool IsGroupActive(string paramId) => 
            GetGroupVolume(paramId) > 0;

        public void SetActiveGroup(string paramId, bool isActive) => 
            SetGroupVolumeByName(paramId, isActive ? 1 : 0);

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

        private void SetGroupVolumeWithoutSaving(string groupName, float normalizedVolume) => 
            _audioMixer.SetFloat(groupName, GetVolume(normalizedVolume));

        private float GetVolume(float value) => 
            Mathf.Lerp(-80, 0, value);
        
        public enum Priority { Default, High }
    }
}