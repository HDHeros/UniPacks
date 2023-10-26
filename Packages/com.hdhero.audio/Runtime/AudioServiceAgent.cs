using UnityEngine;

namespace HDH.Audio
{
    public class AudioServiceAgent : MonoBehaviour
    {
        [Header("Audio Player 3D")] [SerializeField]
        private int _agentsAmount;
        [SerializeField]
        private int _freeAgentsAmount;
        private AudioService _audioService;


        public void Initialize(AudioService audioService)
        {
            _audioService = audioService;
        }

        private void Awake() => 
            DontDestroyOnLoad(this);

#if UNITY_EDITOR
        private void Update()
        {
            if (_audioService == null) return;
            _freeAgentsAmount = _audioService.Player3D.FreeAgentsAmount;
            _agentsAmount = _audioService.Player3D.AgentsAmount;
        }
#endif
    }
}