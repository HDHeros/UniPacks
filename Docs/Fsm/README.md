## Fsm
Fsm is small package that provides one of the variations of state machine in my implementation.
As states are here classes that can override object's behaviour. We'll deal with it bellow.

### Installing the package
#### Via manifest.json
Open `(yourProjectPath)/Packages/manifest.json`, then add the package in the list of dependencies as bellow.

 ```json
 {
  "dependencies": {
    "com.hdhero.fsm": "https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.fsm"
  }
}
```

#### Via Unity Package Manager
1. Open Unity Package Manager

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_1.png)
2. Click `Add Package` and choose `Add package from GIT url...` option

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_2.png)
3. Paste the link `https://github.com/HDHeros/UniPacks.git?path=Packages/com.hdhero.fsm`, then click `Add`

   ![](https://github.com/HDHeros/UniPacks/blob/main/Docs/UserData/userdata_install_viaupm_3.png)

### How to use
В первую очередь нужно создать два класса: базовый класс для ваших будущих состояний и класс, который будет содержать в себе общие поля\свойства для всех состояний.
```c#
[Serializable]
public class FsmExampleFields : IFsmSharedFields
{
   [SerializeField] public Transform Transform;
   [NonSerialized] public string SomeString;
}

public class FsmExampleBaseState : BaseFsmState
{
  public virtual void DoSomeAction(){}
}
```

Дальше мы можем создать конкретные состояний для на