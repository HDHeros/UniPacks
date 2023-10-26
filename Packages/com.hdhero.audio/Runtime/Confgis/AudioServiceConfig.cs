using System;
using UnityEngine.Audio;

namespace HDH.Audio.Confgis
{
    [Serializable]
    public class AudioServiceConfig
    {
        public AudioMixer Mixer;
        public int SourcesLimit2D = 5;
        public int SourcesLimit3D = 5;
        public float MusicTransitionDuration;
        public SceneThemes[] ScenesThemes;
        public string[] MixerVolumeParameters;
        public int PlaybackPrioritiesAmount = 10;

        [Serializable]
        public struct SceneThemes
        {
            public string SceneName;
            public AudioConfig[] Themes;
        }
    }
}