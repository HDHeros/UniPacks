using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDH.GoPool.Components
{
    public class PooledParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        #if UNI_TASK
        
        public async UniTaskVoid PlayAndReturn(
            Vector3 position, 
            PooledParticle selfPrefab, 
            IGoPool pool,
            CancellationToken ct, 
            Quaternion? rotation = null,
            float? delay = null)
        {
            transform.position = position;
            if (rotation.HasValue)
                transform.rotation = rotation.Value;
            gameObject.SetActive(true);
            if (delay.HasValue)
                await UniTask.Delay(TimeSpan.FromSeconds(delay.Value), cancellationToken: ct);
            
            _particle.Play();
            bool cancelled = await UniTask.WaitUntil(() => _particle == null || _particle.isPlaying == false, cancellationToken: ct)
                .SuppressCancellationThrow();
            if (cancelled || _particle == null) return;
            pool.Return(this, selfPrefab);
        }
        
        #endif
    }
}