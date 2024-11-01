using System;
using UnityEngine;

namespace HDH.UserData.Dto
{
    [Serializable]
    public struct UserDataConfig : IUserDataConfig
    {
        public string SaveFolderPath => _saveFolderPath;
        public string FileExtension => _fileExtension;
        public float SaveDelayInSeconds => _saveDelayInSeconds;
        
        [SerializeField] private string _saveFolderPath;
        [SerializeField] private string _fileExtension;
        [SerializeField] private float _saveDelayInSeconds;

        public UserDataConfig(string saveFolderPath, string fileExtension, float saveDelayInSeconds)
        {
            _saveFolderPath = saveFolderPath;
            _fileExtension = fileExtension;
            _saveDelayInSeconds = saveDelayInSeconds;
        }
    }
}