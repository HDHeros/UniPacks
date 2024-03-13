﻿using HDH.Fsm;
using UnityEngine.EventSystems;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public abstract class FsmExampleBaseState : BaseFsmState<FsmExample.SharedFields>
    {
        public virtual void Update() { }

        public virtual void OnPointerClick(PointerEventData eventData) { }
    }
}