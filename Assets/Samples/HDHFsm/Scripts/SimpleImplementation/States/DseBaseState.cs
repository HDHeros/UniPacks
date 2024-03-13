using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using UnityEngine.EventSystems;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public abstract class DseBaseState : IState
    {
        protected IStateSwitcher StateSwitcher { get; private set; }

        protected DseBaseState(IStateSwitcher stateSwitcher) => 
            StateSwitcher = stateSwitcher;

        public virtual void Enter(){}

        public virtual void Exit() { }
        
        public virtual void Update() { }

        public virtual void OnPointerClick(PointerEventData eventData) { }
    }
}