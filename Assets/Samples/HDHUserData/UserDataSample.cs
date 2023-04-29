using System;
using HDH.UserData;
using HDH.UserData.Dto;
using UnityEngine;

namespace Samples.HDHUserData
{
    public class UserDataSample : MonoBehaviour
    {
        [SerializeField] private UserDataConfig _userDataConfig;
        private UserDataService _userData;
        private SampleDataModel1 _model1;
        private SampleDataModel2 _model2;

        public struct TestStruct
        {
            public string Str;
            public int[] Values;
        }
        
        
        private void Start()
        {
            _userData = new UserDataService(_userDataConfig);
            _model1 = _userData.GetModel<SampleDataModel1>();
            _model2 = _userData.GetModel<SampleDataModel2>();
        }

        private void Update()
        {
            _model1.FloatValue += Time.deltaTime;
            _model1.Save();
        }
    }
}
