using System.Collections.Generic;
using HDH.UserData;
using Newtonsoft.Json;

namespace Samples.HDHUserData
{
    public class SampleDataModel2 : DataModel
    {
        [JsonProperty] public string[] SomeArray = new []{"asdas", "asd", "hdfkjhk", "4352jkjk"};
        [JsonProperty] public string[][] DSomeArray = new []{null, null, new string[]{"asdas", "asd", "hdfkjhk", "4352jkjk"}};
        [JsonProperty] public List<UserDataSample.TestStruct> SomeList = new List<UserDataSample.TestStruct>()
        {
            new UserDataSample.TestStruct{Str = "aswghyrt", Values = new []{1,7,4,7,9,5}},
            new UserDataSample.TestStruct{Str = "123", Values = new []{1,7,4,7,9,5}},
            new UserDataSample.TestStruct{Str = "96kjhk", Values = new []{1,7,4,7,9,5}},
            new UserDataSample.TestStruct{Str = "gki", Values = new []{1,7,4,7,9,5}},
        };
    }
}