using UnityEngine;

namespace HDH.UserData
{
    public class SaveLoadMonoAgent : MonoBehaviour
    {
        public UserDataService DataService { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Setup(UserDataService dataService)
        {
            DataService = dataService;
        }
    }
}