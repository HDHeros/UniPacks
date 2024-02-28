using System;
using HDH.Fsm.Debug;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace HDH.FsmEditor.Editor
{
    // [CustomEditor(typeof(FsmDebugger))]
    public class FsmDebuggerEditor : UnityEditor.Editor
    {
        private FsmDebugger _debugger;
        private string[] _strings;

        private void OnEnable()
        {
            _debugger = (FsmDebugger)target;
        }
    }
}