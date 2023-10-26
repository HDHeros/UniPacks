using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace HDH.Audio.Confgis
{
    [CreateAssetMenu(menuName = "HDH/Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        #if UNITY_EDITOR
        public event Action Validated;
        #endif
        
        #region Properties

        public AudioClip AudioClip => GetClip();
        public int VolumePriority => _volumePriority;
        public AudioMixerGroup Output => _output;
        public bool LimitPlaybacksCount => _limitPlaybacksCount;
        public int MaxParallelPlaybacksCount => _maxParallelPlaybacksCount;
        public bool Mute => _mute;
        public bool BypassEffects => _bypassEffects;
        public bool BypassListenerEffects => _bypassListenerEffects;
        public bool BypassReverbZones => _bypassReverbZones;
        public bool Loop => _loop;
        public int Priority => _priority;
        public float Volume => _volume;
        public float Pitch => Random.Range(_pitchMin, _pitchMax);
        public float StereoPan => _stereoPan;
        public float SpatialBlend => _spatialBlend;
        public float ReverbZoneMix => _reverbZoneMix;
        public float DopplerLevel => _dopplerLevel;
        public float Spread => _spread;
        public AudioRolloffMode RolloffMode => _rolloffMode;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public float AudioClipsAmount => _audioClips.Length;
        #endregion
        
        #region Serialize fields
        [SerializeField] private ClipWrapper[] _audioClips;
        [SerializeField] private int _volumePriority;
        [SerializeField] private AudioMixerGroup _output;
        [SerializeField] private bool _limitPlaybacksCount;
        [SerializeField] private int _maxParallelPlaybacksCount;
        [SerializeField] private bool _mute;
        [SerializeField] private bool _bypassEffects;
        [SerializeField] private bool _bypassListenerEffects;
        [SerializeField] private bool _bypassReverbZones;
        [SerializeField] private bool _loop;
        [SerializeField] [Range(0, 256)] private int _priority = 128;
        [SerializeField] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] [Range(-3f, 3f)] private float _pitchMin = 1f;
        [SerializeField] [Range(-3f, 3f)] private float _pitchMax = 1f;
        [SerializeField] [Range(-1f, 1f)] private float _stereoPan;
        [SerializeField] [Range(0f, 1f)] private float _spatialBlend;
        [SerializeField] [Range(0f, 1.1f)] private float _reverbZoneMix = 1f;
        [SerializeField] [Range(0f, 5f)] private float _dopplerLevel = 1f;
        [SerializeField] [Range(0f, 360f)] private float _spread;
        [SerializeField] private AudioRolloffMode _rolloffMode;
        [SerializeField] private float _minDistance = 1f;
        [SerializeField] private float _maxDistance = 500f;
    #endregion

        public void ConfigureSource(AudioSource source)
        {
            source.clip = AudioClip;
            source.outputAudioMixerGroup = Output;
            source.mute = Mute;
            source.bypassEffects = BypassEffects;
            source.bypassListenerEffects = BypassListenerEffects;
            source.bypassReverbZones = BypassReverbZones;
            source.loop = Loop;
            source.priority = Priority;
            source.volume = Volume;
            source.pitch = Pitch;
            source.panStereo = StereoPan;
            source.spatialBlend = SpatialBlend;
            source.reverbZoneMix = ReverbZoneMix;
            source.dopplerLevel = DopplerLevel;
            source.spread = Spread;
            source.rolloffMode = RolloffMode;
            source.minDistance = MinDistance;
            source.maxDistance = MaxDistance;
        }
        
        private AudioClip GetClip()
        {
            float probabilitySum = _audioClips.Sum(c => c.Probability);
            float randomValue = Random.Range(0f, probabilitySum);
            foreach (ClipWrapper wrapper in _audioClips)
            {
                if (wrapper.Probability >= randomValue) return wrapper.AudioClip;
                randomValue -= wrapper.Probability;
            }

            throw new NullReferenceException($"Missing audio clip in {name}");
        }
        
        private void OnValidate()
        {
            _minDistance = Mathf.Clamp(_minDistance, 0, _maxDistance * 0.99f);
            _maxDistance = Mathf.Clamp(_maxDistance, 0.01f, float.MaxValue);
            _pitchMax = Mathf.Clamp(_pitchMax, _pitchMin, 3);
            _pitchMin = Mathf.Clamp(_pitchMin, -3, _pitchMax);
            for(int i = 0; i < _audioClips.Length; i++)
            {
                _audioClips[i].Probability = Mathf.Clamp(_audioClips[i].Probability, 0f, float.MaxValue);
            }
#if UNITY_EDITOR
        
            Validated?.Invoke();
#endif
        }

        [Serializable]
        public struct ClipWrapper
        {
            public AudioClip AudioClip;
            public float Probability;
        }
    }
}