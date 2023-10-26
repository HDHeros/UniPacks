using System.Collections.Generic;
using HDH.Audio.Confgis;

namespace HDH.Audio.Player3D
{
    public class VolumePrioritizer
    {
        private readonly List<Player3DAgent> _playingAgents;
        private readonly int _maxParallelPlayingPriorities;
        
        public VolumePrioritizer(List<Player3DAgent> agents, AudioServiceConfig config)
        {
            _playingAgents = new List<Player3DAgent>(agents.Capacity);
            _maxParallelPlayingPriorities = config.PlaybackPrioritiesAmount;
        }

        public void OnNewAgentInstantiated(Player3DAgent agent)
        {
            agent.PlaybackStarted += OnAgentPlaybackStarted;
            agent.PlaybackStopped += OnAgentPlaybackStopped;
        }

        private void SetPriorities()
        {
            int iteratedPrioritiesCount = 0;
            float volume = 1;
            int prevPriority = int.MaxValue;
            for (var i = _playingAgents.Count - 1; i >= 0; i--)
            {
                if (_playingAgents[i].AudioConfig.VolumePriority < prevPriority)
                {
                    iteratedPrioritiesCount++;
                    prevPriority = _playingAgents[i].AudioConfig.VolumePriority;
                    volume = iteratedPrioritiesCount <= _maxParallelPlayingPriorities ? 1 : 0;
                }
                
                _playingAgents[i].SetVolumeMultiplier(volume);
            }
        }

        private void OnAgentPlaybackStarted(Player3DAgent agent)
        {
            if (_playingAgents.Contains(agent)) return;
            int iterationsCount = _playingAgents.Count + 1;
            for (var i = 0; i < iterationsCount; i++)
            {
                if (i == iterationsCount - 1 || 
                    _playingAgents[i].AudioConfig.VolumePriority >= agent.AudioConfig.VolumePriority)
                {
                    _playingAgents.Insert(i, agent);
                    break;
                }
            }

            SetPriorities();
        }

        private void OnAgentPlaybackStopped(Player3DAgent agent)
        {
            _playingAgents.Remove(agent);
            SetPriorities();
        }
    }
}