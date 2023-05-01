using System.Collections.Generic;
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

        public void OnValidate()
        {
            HashSet<string> names = new HashSet<string>();
            for (var i = 0; i < _constants.Length; i++)
            {
                ref EnumConst c = ref _constants[i];
                if (names.Contains(c.Name))
                {
                    c.IsNameValid = false;
                    continue;
                }

                names.Add(c.Name);
                c.IsNameValid = true;
            }
        }
    }
}