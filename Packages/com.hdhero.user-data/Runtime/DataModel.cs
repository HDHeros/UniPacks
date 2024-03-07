using System;

namespace HDH.UserData
{
    [Serializable]
    public class DataModel
    {
        public event Action<DataModel> SaveRequested;
        public event Action<DataModel> ForceSaveRequested;

        public void Save() => 
            SaveRequested?.Invoke(this);

        public void ForceSave() => 
            ForceSaveRequested?.Invoke(this);
        
        public virtual void BeforeSave(){}
    }
}