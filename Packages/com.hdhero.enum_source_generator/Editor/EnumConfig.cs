using UnityEngine;

namespace HDH.ESG.Editor
{
    [CreateAssetMenu(fileName = "New Enum Config", menuName = "HDH/ESG/Enum Config")]
    internal class EnumConfig : ScriptableObject
    {
        [SerializeField] private EnumConst[] _consts;
    }
}