using HDH.Audio;
using HDH.Audio.Confgis;
using UnityEngine;

namespace Samples.HDHAudio.Scripts
{
    public class AudioServiceLauncher : MonoBehaviour
    {
        [SerializeField] private AudioServiceConfig _config;
        public AudioService Audio { get; private set; }

        private void Awake() => 
            Audio = new AudioService(_config);
    }
}