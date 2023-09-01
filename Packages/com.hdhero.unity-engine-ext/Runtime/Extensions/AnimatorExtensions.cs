using System.Threading;
using UnityEngine;
#if UNI_TASK
using Cysharp.Threading.Tasks;
#endif

namespace HDH.UnityExt.Extensions
{
    public static class AnimatorExtensions
    {
#if UNI_TASK
        /// <summary>
        /// Await until name of target animator's current state equals "stateName"  
        /// </summary>
        /// <param name="target">Target animator</param>
        /// <param name="stateName">Name of awaited state</param>
        /// <param name="ct"></param>
        /// <param name="layerIndex">Target animator's layer index</param>
        /// <returns></returns>
        public static UniTask AwaitAnimatorState(this Animator target, string stateName, CancellationToken ct, int layerIndex = 0)
        {
            return UniTask.WaitUntil(() => target.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName),
                cancellationToken: ct);
        }
#endif
    }
}