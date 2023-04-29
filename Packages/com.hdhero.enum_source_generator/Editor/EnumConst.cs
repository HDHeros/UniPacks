using System;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [Serializable]
    internal struct EnumConst
    {
        [SerializeField] internal string Name;
        [SerializeField] internal bool SetValueExplicit;
        [SerializeField] internal int Value;
    }
}