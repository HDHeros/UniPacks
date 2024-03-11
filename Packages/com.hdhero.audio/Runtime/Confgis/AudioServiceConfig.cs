using System;
using HDH.Audio.Player3D;
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
        public string[] MixerVolumeParameters;
        public int PlaybackPrioritiesAmount = 10;
        public Type Player3DAgentType = typeof(Player3DAgent);
    }
}