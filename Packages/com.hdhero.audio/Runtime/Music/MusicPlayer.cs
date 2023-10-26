using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HDH.Audio.Confgis;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HDH.Audio.Music
{
   public class MusicPlayer
   {
      private readonly AudioServiceConfig _config;
      private readonly AudioSource _source1;
      private readonly AudioSource _source2;
      private readonly MusicPlayerAgent _agent;
      private AudioSource _currentlyPlayingSource;
      private Scene _currentScene;
      private CancellationTokenSource _playerCancellation;
      private AudioConfig _forcedTrack;


      public MusicPlayer(AudioService audioService, AudioServiceConfig config)
      {
         _config = config;
         _agent = new GameObject("Music Player").AddComponent<MusicPlayerAgent>();
         _agent.transform.SetParent(audioService.Transform);
         _source1 = _currentlyPlayingSource = _agent.gameObject.AddComponent<AudioSource>();
         _source2 = _agent.gameObject.AddComponent<AudioSource>();
         SceneManager.sceneLoaded += OnSceneLoaded;
      }

      public void DisableForcedTrack() => 
         ForcePlayTrack(null);
      
      public void ForcePlayTrack(AudioConfig track)
      {
         _forcedTrack = track;
         ForceChangeTrack();
      }
      
      private async UniTaskVoid StartPlayLoop(CancellationToken cancellation)
      {
         while (cancellation.IsCancellationRequested == false)
         {
            if (TryGetNextTrack(out var data) == false)
            {
               _currentlyPlayingSource.volume = 0;
               return;
            }
            
            PlayTrack(data);
            await UniTask.Delay(
               TimeSpan.FromSeconds(_currentlyPlayingSource.clip.length - _config.MusicTransitionDuration), 
               cancellationToken: cancellation);
         }
      }

      private void ForceChangeTrack()
      {
         _playerCancellation?.Cancel();
         _playerCancellation = new CancellationTokenSource();
         StartPlayLoop(_playerCancellation.Token).Forget();
      }
      
      private void PlayTrack(AudioConfig track)
      {
         var playingSource = _currentlyPlayingSource == _source1 ? _source1 : _source2;
         var freeSource = _currentlyPlayingSource == _source1 ? _source2 : _source1;

         _agent.StopAllCoroutines();

         _agent.StartCoroutine(FadeAudioSource(playingSource, 0, _config.MusicTransitionDuration, playingSource.Stop));
         track.ConfigureSource(freeSource);
         
         freeSource.Play();
         _agent.StartCoroutine(FadeAudioSource(freeSource, 1, _config.MusicTransitionDuration));
         _currentlyPlayingSource = freeSource;
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
   
      private bool TryGetNextTrack(out AudioConfig track)
      {
         track = null;
         if (_forcedTrack != null)
         {
            track = _forcedTrack;
            return true;
         }
         
         var themes = _config.ScenesThemes.FirstOrDefault(d => d.SceneName.Equals(_currentScene.name));
         if (themes.Themes == null || themes.Themes.Length == 0) return false;
         track = themes.Themes[Random.Range(0, themes.Themes.Length)];
         return true;
      }

      private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
      {
         _currentScene = arg0;
         _forcedTrack = null;
         ForceChangeTrack();
      }
   }
}