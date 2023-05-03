## ESG
ESG (Enum Source Generator) is a simple used tool for generation of enum source code from Unity Editor.  

### Installing the package 
**Via manifest.json**
Open `(yourProjectPath)/Packages/manifest.json`, then add the package in the list of dependencies as bellow.

 ```json
 {
  "dependencies": {
    "com.hdhero.enum_source_generator": "https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.enum_source_generator"
  }
}
```

**Via Unity Package Manager**
1. Open Unity Package Manager

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_1.png)
2. Click `Add Package` and choose `Add package from GIT url...` option

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_2.png)
3. Paste the link `https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.user-data`, then click `Add`

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_3.png)

### How to use?
1. Create new enum's config `Create => HDH => ESG => Enum Config`  

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/EnumGen/esg_create-config.png)  
2. Use `Add` button to add items to list  

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/EnumGen/esg_items-list.png)  
3. Fill Name  
4. If you want to specify item's value explicit you should to turn on `SetValueExplicit` and fill `Value`  
5. Use remove button to remove item from list  
6. You can sort items by name or value using the corresponding buttons  
7. Enter type name. You can use either simple name like `MyEnum` or specify full name with namespace `MyProject.Feature.MyEnum`   

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/EnumGen/esg_items-list.png)  
8. Chose folder to save file in. Click button `...` to open "open folder dialog"  
9. If all is good you can click `Create` or `Update` button and file with enum will be created  

Done! Your enum is generated, you can find the file in specified folder and use enum constants from scripts you need

