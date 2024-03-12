using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDH.GoPool.Components
{
    public class PooledParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        public ParticleSystem ParticleSystem => _particle;

        #if UNI_TASK
        
        public async UniTaskVoid PlayAndReturn(
            Vector3 position, 
            PooledParticle selfPrefab, 
            IGoPool pool,
            CancellationToken ct, 
            Quaternion? rotation = null,
            float? delay = null)
        {
            CancellationTokenRegistration registration = ct.Register(OnPlayed);
            transform.position = position;
            if (rotation.HasValue)
                transform.rotation = rotation.Value;
            gameObject.SetActive(true);
            if (delay.HasValue)
                await UniTask.Delay(TimeSpan.FromSeconds(delay.Value), cancellationToken: ct);
            
            _particle.Play();
            await UniTask.WaitUntil(() => _particle == null || _particle.isPlaying == false, cancellationToken: ct);
            registration.Dispose();
            OnPlayed();
            
            void OnPlayed()
            {
                if (_particle == null || _particle.gameObject == null) return;
                pool.Return(this, selfPrefab);
            }
        }
        
        #endif
    }
}