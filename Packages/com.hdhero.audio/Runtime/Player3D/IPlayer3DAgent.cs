using HDH.Audio.Confgis;
using UnityEngine;

namespace HDH.Audio.Player3D
{
    public interface IPlayer3DAgent
    {
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public void Play(AudioConfig config, Transform client, bool returnAfterPlay = true, bool synchronizePosition = true, float synchronizeRate = 0.2f);
        public void Configure();
        public void Stop();
        public void Return();
    }
}