using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CreateAssetMenu(fileName = "New Enum Config", menuName = "HDH/ESG/Enum Config")]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    internal class EnumConfig : ScriptableObject
    {
        [SerializeField] public string EnumName;
        [SerializeField] public string FolderPath;
        [SerializeField] public EnumConst[] Constants;
    }
}