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
Рассмотрим на примере уже существующего в проекте сэмпла.
Есть некоторая многофункциональная кнопка, которая может переходить между состояниями и в каждом из стетов клик по ней будет иметь разный эффект.

Для начала создадим базовый класс для состояний
```c#
 public abstract class FsmExampleBaseState : BaseFsmState<FsmExample.SharedFields>
 {
     public virtual void Update() { }

     public virtual void OnPointerClick(PointerEventData eventData) { }
 }
```
У него сделали два виртуальных метода, которые в адльнейшем будут переопределены дочерними состояниями.

Далее нужно сделать класс - общее хранилище данных объекта, которые будут доступны всем состояниям. Объявим его прямо внутри класса монобеха кнопки.

```c#
 public class FsmExample : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SharedFields _fields;

        private Fsm<FsmExampleBaseState, SharedFields> _fsm;
        
        public void OnPointerClick(PointerEventData eventData) => 
            _fsm.CurrentState.OnPointerClick(eventData);

        private void Awake()
        {
            _fields.CoroutineRunner = this;
            _fsm = Fsm<FsmExampleBaseState, SharedFields>
                .Create(_fields)
                .AddState<FsmExampleAwaitToLoadingStartState>()
                .AddState<FsmExampleLoadingState>()
                .AddState<FsmExampleGameState>()
                .AddState<FsmExampleGameFinishedState>()
                .Start();
        }
        
        private void Update() => 
            _fsm.CurrentState.Update();

        [Serializable]
        public class SharedFields : IFsmSharedFields
        {
            public Text Label;
            public RectTransform LockPanel;
            public RectTransform GamePanel;
            [NonSerialized] public MonoBehaviour CoroutineRunner;
            [NonSerialized] public int ClicksCounter;
            [NonSerialized] public float LoadingProgress;
        }
     }
```
Тут же делегируем обработку кликов и метода апдейт состояниям, создадим поля для машины состояний и общих полей, создадим и запустим машину состояний.
Реализацию состояний можно подсмотреть [тут](https://github.com/HDHeros/UniPacks/tree/fsm_debugger/Assets/Samples/HDHFsm/Scripts/States)

Можно переходить в юнити и тестировать.

### How to debug in runtime
Также предусмотрен дебаггер, который на данный момент позволяет мониторить текущее состояние и поля FSM
Чтобы им воспользоваться нужно добавить компонент FsmDebugger объекту, реализовать интерфейс IFsmContainer и прокинуть ссылку на геймобжект в инспекторе.
Окно дебага отображается только при объект с дебаггером активен в окне иерархии