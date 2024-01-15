using System;
using System.Collections;
using System.Collections.Generic;
using HDH.UserData.Dto;
using UnityEngine;

namespace HDH.UserData
{
    public class UserDataService
    {
        private static bool s_isInstanceExist;
        private readonly ISaveLoadService _saveLoadService;
        private readonly Dictionary<Type, DataModelInfo> _loadedModels;
        private readonly SaveLoadMonoAgent _monoAgent;
        private readonly WaitForSeconds _saveDelayYield;

        public Dictionary<Type, DataModelInfo> LoadedModels => _loadedModels;

        public UserDataService(IUserDataConfig config, ISaveLoadService saveLoadService = null)
        {
            if (s_isInstanceExist)
                throw new Exception($"An instance of {nameof(UserDataService)} is already exist.");

            s_isInstanceExist = true;
            _saveLoadService = saveLoadService ?? new SaveLoadService(config.SaveFolderPath, config.FileExtension);
            _loadedModels = new Dictionary<Type, DataModelInfo>();
            _monoAgent = new GameObject("[SaveLoadAgent]").AddComponent<SaveLoadMonoAgent>();
            _monoAgent.Setup(this);
            _saveDelayYield = new WaitForSeconds(config.SaveDelayInSeconds);
        }

        public T GetModel<T>() where T : DataModel, new()
        {
            if (_loadedModels.TryGetValue(typeof(T), out var info))
                return (T) info.Model;

            info = new DataModelInfo {Model = _saveLoadService.Load<T>()};
            _loadedModels.Add(typeof(T), info);
            info.Model.SaveRequested += Save;
            info.Model.ForceSaveRequested += ForceSaveModel;
            return (T) info.Model;
        }

        public void Save<T>(T model) where T : DataModel, new()
        {
            ValidateModel(model);
            DataModelInfo modelInfo = _loadedModels[model.GetType()];
            if (modelInfo.IsDirty) return;
            modelInfo.IsDirty = true;
            _monoAgent.StartCoroutine(SaveDelayed(model, modelInfo));
        }

        public void ForceSaveModel<T>(T model) where T : DataModel, new()
        {
            ValidateModel(model);
            _saveLoadService.Save(model);
        }

        private void ValidateModel(DataModel model)
        {
            if (_loadedModels.TryGetValue(model.GetType(), out var modelInfo) == false
                || modelInfo.Model == model == false)
                throw new ArgumentException("Model you try to save wasn't created by this UserData and can't be saved");
        }

        private IEnumerator SaveDelayed<T>(T model, DataModelInfo modelInfo) where T : DataModel, new()
        {
            yield return _saveDelayYield;
            _saveLoadService.Save(model);
            modelInfo.IsDirty = false;
        }
    
        public class DataModelInfo
        {
            public DataModel Model;
            public bool IsDirty;
        }
    }
}

