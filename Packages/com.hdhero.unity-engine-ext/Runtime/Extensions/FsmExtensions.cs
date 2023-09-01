#if HDH_FSM
using HDH.Fsm;
#if UNI_TASK
using Cysharp.Threading.Tasks;
#endif

namespace HDH.UnityExt.Extensions
{
    public static class FsmExtensions
    {
#if UNI_TASK
        /// <summary>
        /// Switch state and await until target state is current state
        /// </summary>
        /// <param name="fsm">Target fsm</param>
        /// <typeparam name="TBaseState">Type of base state of target FSM</typeparam>
        /// <typeparam name="TFields">Shared fields' type of target fsm</typeparam>
        /// <typeparam name="TTargetState">Target state's type</typeparam>
        /// <returns></returns>
        public static UniTask SwitchStateAndAwait<TBaseState, TFields, TTargetState>(this Fsm<TBaseState, TFields> fsm) 
            where TTargetState : TBaseState
            where TBaseState : BaseFsmState<TFields>
            where TFields : IFsmSharedFields
        {
            bool switched = false;
            fsm.SwitchState<TTargetState>(() => switched = true);
            return fsm.CurrentState is TTargetState ? UniTask.CompletedTask : UniTask.WaitUntil(() => switched);
        }
#endif
    }
}
#endif
