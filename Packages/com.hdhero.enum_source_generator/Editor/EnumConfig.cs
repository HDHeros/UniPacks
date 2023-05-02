using System.Linq;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CreateAssetMenu(fileName = "New Enum Config", menuName = "HDH/ESG/Enum Config")]
    internal class EnumConfig : ScriptableObject
    {
        [SerializeField] private EnumConst[] _constants;

        public void Sort()
        {
            _constants = _constants.OrderBy(c => c.Value).ToArray();
        }
    }
}