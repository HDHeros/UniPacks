using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace HDH.UserData
{
    internal class SaveLoadService : ISaveLoadService
    {
        public event Action Initialized;
        public bool IsInitialized => true;
        private readonly string _path;
        private readonly string _fileExtension;
        private readonly BinaryFormatter _binaryFormatter;


        internal SaveLoadService(string saveFolder, string fileExtension)
        {
            _path = $"{Application.persistentDataPath}/{saveFolder}";
            _fileExtension = fileExtension;
            _binaryFormatter = new BinaryFormatter();
        }

        public void Save<T>(T model) where T : DataModel, new()
        {
            string fileFullPath = GetFileFullPath(model.GetType());
            var stream = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.Write);
            var data = JsonConvert.SerializeObject(model);
            _binaryFormatter.Serialize(stream, data);
            stream.Close();
        }

        public T Load<T>() where T : DataModel, new()
        {
            string fullPath = GetFileFullPath(typeof(T));
            if (Directory.Exists(_path) == false)
                Directory.CreateDirectory(_path);
            return File.Exists(fullPath) ? Deserialize<T>(fullPath) : Instantiate<T>();
        }

        private string GetFileFullPath(Type type) => 
            $"{_path}/{type.Name}.{_fileExtension}";

        private T Deserialize<T>(string fullPath) where T : DataModel, new()
        {
            var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read);
            var data = _binaryFormatter.Deserialize(stream).ToString();
            stream.Close();
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Couldn't  deserialize {fullPath}. Instantiated new {typeof(T).Name}. {e.Message}");
                return Instantiate<T>();
            }
        }

        private T Instantiate<T>() where T : DataModel, new() => 
            new T();
    }
}