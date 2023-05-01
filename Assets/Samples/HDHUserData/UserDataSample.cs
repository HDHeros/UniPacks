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
        private SampleDataModel _model;
        private SampleDataModel2 _model2;

        public struct TestStruct
        {
            public string Str;
            public int[] Values;
        }
        
        private void Awake()
        {
            _userData = new UserDataService(_userDataConfig);
            _model = _userData.GetModel<SampleDataModel>();
            _model2 = _userData.GetModel<SampleDataModel2>();
        }

        private void Update()
        {
            _model.FloatValue += Time.deltaTime;
            _model.Save();
            _model.Stroka = "VeryImportantStringThatShouldBeForceSaved";
            _model.ForceSave();
        }
    }
}
