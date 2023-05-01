## User Data
UserData is a simple used service for your Unity Application helps you to save and load user's data. The service serializes and stores data as JSON files in presistant data path.
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