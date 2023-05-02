using UnityEngine;

namespace HDH.ESG.Editor
{
    [CreateAssetMenu(fileName = "New Enum Config", menuName = "HDH/ESG/Enum Config")]
    internal class EnumConfig : ScriptableObject
    {
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private EnumConst[] _constants;
    }
}