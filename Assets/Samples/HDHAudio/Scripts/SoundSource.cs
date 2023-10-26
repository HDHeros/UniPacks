using HDH.Audio.Confgis;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.HDHAudio.Scripts
{
    public class SoundSource : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioServiceLauncher _audio;
        [SerializeField] private AudioConfig _soundConfig;
        [SerializeField] private bool _is3D;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_is3D)
                _audio.Audio.Player3D.Play(_soundConfig, transform);
            else
                _audio.Audio.Player.PlaySound(_soundConfig);
        }
    }
}