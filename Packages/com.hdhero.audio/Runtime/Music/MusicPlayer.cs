using System;
using System.Collections;
using HDH.Audio.Confgis;
using UnityEngine;

namespace HDH.Audio.Music
{
   public class MusicPlayer
   {
      public event Action CurrentSongEnds;
      private readonly AudioServiceConfig _config;
      private readonly AudioSource _source1;
      private readonly AudioSource _source2;
      private readonly MusicPlayerAgent _agent;
      private AudioSource _currentlyPlayingSource;
      private Coroutine _currentWatchingCoroutine;


      public MusicPlayer(AudioService audioService, AudioServiceConfig config)
      {
         _config = config;
         _agent = new GameObject("Music Player").AddComponent<MusicPlayerAgent>();
         _agent.transform.SetParent(audioService.Transform);
         _source1 = _currentlyPlayingSource = _agent.gameObject.AddComponent<AudioSource>();
         _source2 = _agent.gameObject.AddComponent<AudioSource>();
      }

      public void Play(AudioConfig themeConfig, bool withFade = true) => 
         PlayTrack(themeConfig, withFade);

      public void Stop()
      {
         if (_currentWatchingCoroutine != null)
            _agent.StopCoroutine(_currentWatchingCoroutine);
         _currentlyPlayingSource.Stop();
      }

      private void PlayTrack(AudioConfig track, bool withFade)
      {
         if (_currentWatchingCoroutine != null) 
            _agent.StopCoroutine(_currentWatchingCoroutine);
         
         var playingSource = _currentlyPlayingSource == _source1 ? _source1 : _source2;
         var freeSource = _currentlyPlayingSource == _source1 ? _source2 : _source1;

         _agent.StopAllCoroutines();
         
         _agent.StartCoroutine(FadeAudioSource(playingSource, 0, withFade ? _config.MusicTransitionDuration : 0, playingSource.Stop));
         track.ConfigureSource(freeSource);

         if (withFade)
            freeSource.volume = 0;
         freeSource.Play();
         _agent.StartCoroutine(FadeAudioSource(freeSource, track.Volume, withFade ? _config.MusicTransitionDuration : 0));
         _currentlyPlayingSource = freeSource;
         _currentWatchingCoroutine = _agent.StartCoroutine(WatchCurrentSource());
      }

      private IEnumerator WatchCurrentSource()
      {
         while (_currentlyPlayingSource.isPlaying)
         {
            if (_currentlyPlayingSource.clip.length - _currentlyPlayingSource.time <= _config.MusicTransitionDuration)
            {
               CurrentSongEnds?.Invoke();
               yield break;
            }

            yield return null;
         }
      }

      private IEnumerator FadeAudioSource(AudioSource audioSource, float value, float duration, Action onComplete = null)
      {
         float currentDuration = 0;
         float startVolume = audioSource.volume;
         while (currentDuration <= duration)
         {
            audioSource.volume = Mathf.Lerp(startVolume, value, currentDuration / duration);
            yield return null;
            currentDuration += Time.deltaTime;
         }

         audioSource.volume = value;
         onComplete?.Invoke();
      }
   }
}