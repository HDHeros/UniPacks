using System;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [Serializable]
    internal struct EnumConst
    {
        [SerializeField] public string Name;
        [SerializeField] public bool SetValueExplicit;
        [SerializeField] public int Value;
        [SerializeField] public bool IsNameValid;
    }
}