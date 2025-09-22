//using UnityEngine;
////using Jab;
using BreakoutGame;
using GameCtor.DevToolbox;
using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    [ServiceProvider]
    [Transient(typeof(int), Factory = nameof(GetInt2))]
    [Singleton(typeof(object), Instance = nameof(TheInt))]
    [Singleton(typeof(ServiceA))]
    //[Singleton(typeof(ServiceB))]
    [Singleton(typeof(TestMono), Factory = nameof(GetTestMono))]
    public partial class CompositionRoot : BaseCompositionRoot
    {
        public TestMono _testMono;
        public object TheInt { get; } = 42;
        public int GetInt2() => 45;
        public TestMono GetTestMono() => _testMono;

        public ExtraLifePowerUpFactory GetExtraLifePowerUpFactory(Game2 game)
        {
            return new ExtraLifePowerUpFactory(game);
        }
    }
}

public struct TestStruct
{
    public int Value;
}

//[ServiceProvider]
//[Transient(typeof(GameObject), Factory = nameof(Go))]
////[Singleton(typeof(Rigidbody), Instance = nameof(Rb))]
////[Import(typeof(MyModule))]
//[Transient(typeof(IService2), typeof(Service2))]
//[Singleton(typeof(IService1), Factory = MyConst)]
//public partial class MyMonoServiceProvider : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject _gohello;
//    [SerializeField]
//    private GameObject go_hello2;
//    [SerializeField]
//    private GameObject go;
//    [SerializeField]
//    private GameObject prefab;
//    [SerializeField]
//    private Rigidbody rb;

//    public GameObject Go() => go;
//    public GameObject Prefab() => prefab;
//    public Rigidbody Rb => rb;
//    public IService1 GetS2() => new Service1();
//    public const string MyConst = "GetS2";
//}

//[ServiceProviderModule]
//[Transient(typeof(GameObject), Factory = nameof(Go))]
//[Transient(typeof(IService1), typeof(Service1))]
//public interface MyModule
//{
//    public static GameObject Go(IService1 service) => new GameObject("generated");
//}
