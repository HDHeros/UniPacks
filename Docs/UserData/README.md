## User Data
UserData is a simple used service for your Unity Application helps you to save and load user's data. The service serializes and stores data as JSON files in persistant data path.
There is an external dependency in service you should to install before use this package - [Newtonsoft.Json-for-Unity](https://github.com/jilleJr/Newtonsoft.Json-for-Unity#upm)

### Installing the package (via manifest.json)
Open `(yourProjectPath)/Packages/manifest.json`, then add the package in the list of dependencies as bellow.

 ```json
 {
  "dependencies": {
    "com.hdhero.user-data": "https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.user-data"
  }
}
```

### Installing the package (via Unity Package Manager)
1. Open Unity Package Manager
   
   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_1.png)
2. Click `Add Package` and choose `Add package from GIT url...` option  
   
   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_2.png)
3. Paste the link `https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.user-data`, then click `Add`  
   
   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_3.png)

### Initialization
For init service you should to create instance if UserData and pass it something implementing `IUserDataConfig` interface. You can use `UserDataConfig` structure
```c#
   public class UserDataSample : MonoBehaviour
   {
      [SerializeField] private UserDataConfig _userDataConfig;
      
      private void Awake()
      {
        _userData = new UserDataService(_userDataConfig);
      }
   }
```

For implement `IUserDataConfig` you should define three properties like
1. `SaveDelayInSeconds` - delay between save model request and saving. Rewriting data file too frequently could be so expensive and adversely affect to performance. Use delay for rewrite file delayed
2. `SaveFolderPath` - path to save data files relative `Application.persistentDataPath`. You can leave it blank, or enter path like `UserData\Models`
3. `FileExtension` - saved files extension**without dot before!** (`txt` is good and `.txt` is bad)

### How to use?
1. First you should to create a subclass of `DataModel` and define properties you need in
```c#
   public class SampleDataModel : DataModel
   {
      [JsonProperty] public int Integer;
      [JsonProperty] public bool Boolean;
      [JsonProperty] public float FloatValue;
      [JsonProperty] public string Stroka = "Stroka";
   }
```
2. Then you can obtain the model
```c#
   ...
   public void Awake()
   {
      _userData = new UserDataService(_userDataConfig);
      _model = _userData.GetModel<SampleDataModel>();
   }
```
3. And Update data and save data
```c#
   public void Update()
   {
      _model.FloatValue += Time.deltaTime;
      _model.Save();
   }
```
Data will be saved with the delay you specified in `UserDataConfig`. It means, that if you Save some data and close the app before delay will pass, your data is not saved.  

4. If you need to save the data without delay you can use `ForceSave()` method
```c#
   public void SaveImportantData()
   {
      _model.Stroka = "VeryImportantStringThatShouldBeForceSaved";
      _model.ForceSave();
   }
```

### Saved data inspection
The service is instantiating a `DontDestroyOnLoad` instance immediately after creation itself in order to use it own purposes.  

![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_agent-instance.png)

Also created object allows you to see currently obtained(!) models and their saved data.  

![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_inspection.png)

