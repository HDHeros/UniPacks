using HDH.UserData;
using Newtonsoft.Json;

namespace Samples.HDHUserData
{
    public class SampleDataModel : DataModel
    {
        [JsonProperty] public int Integer;
        [JsonProperty] public bool Boolean;
        [JsonProperty] public float FloatValue;
        [JsonProperty] public string Stroka = "Stroka";
    }
}