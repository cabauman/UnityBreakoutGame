//using UnityEngine;
////using Jab;
using GameCtor.DevToolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    [ServiceProvider]
    [Singleton(typeof(uint), Factory = nameof(GetInt2))]
    //[Singleton(typeof(object), Instance = nameof(TheInt))]
    //[Singleton(typeof(ServiceA))]
    ////[Singleton(typeof(ServiceB))]
    //[Singleton(typeof(TestMono), Factory = nameof(GetTestMono))]
    //[Singleton(typeof(IPowerUpSpawner), Factory = nameof(GetPowerUpSpawner))]
    //[Scoped(typeof(IRandom), typeof(UnityRandom))]
    ////[Singleton(typeof(Game))]
    //[Singleton(typeof(BrickManager), Instance = nameof(_brickManager))]
    //[Singleton(typeof(BallManager), Instance = nameof(_ballManager))]
    ////[Singleton(typeof(ScoreKeeper))]
    //[Singleton(typeof(Paddle), Instance = nameof(_paddle))]
    ////[Singleton(typeof(PowerUpAction), typeof(ExtraLifePowerUpAction), Key = nameof(PowerUpKind.ExtraLife))]
    public partial class MyMonoServiceProvider : BaseCompositionRoot
    {
        public TestMono _testMono;
        public PowerUpTable _powerUpTable;

        [SerializeField] private Paddle _paddle;
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;

        public object TheInt { get; } = 42;
        public uint GetInt2() => 45;
        public TestMono GetTestMono() => _testMono;

        private PowerUpSpawner GetPowerUpSpawner()
        {
            var dataList = new List<PowerUpData>();
            foreach (var config in _powerUpTable.Configs)
            {
                var command = Resolve<PowerUpAction>(config.Kind.ToString());
                var data = new PowerUpData(config, command);
                dataList.Add(data);
                //InjectDependencies(config);
            }

            return new PowerUpSpawner(
                dataList,
                new PowerUpFactory(),
                GetService<IRandom>());
        }

        private void InjectDependencies(object obj)
        {
            var getServiceMethodBase = typeof(BaseCompositionRoot).GetMethod("Resolve");
            var targetType = obj.GetType();
            var allMethods = targetType.GetMethods();
            var injectMethodsList = new List<MethodInfo>();
            foreach (var method in allMethods)
            {
                if (method.Name == "Inject")
                {
                    injectMethodsList.Add(method);
                }
            }
            var injectMethods = injectMethodsList;
            if (injectMethods.Count == 0)
            {
                UnityEngine.Debug.Log($"No Inject method found for {targetType.FullName}");
                return;
            }

            var currentType = targetType;
            MethodInfo injectMethod = null;
            while (injectMethod is null)
            {
                injectMethod = injectMethods.FirstOrDefault(x => x.DeclaringType == currentType);
                currentType = currentType.BaseType;
            }

            var parameters = injectMethod.GetParameters();
            var injectArgs = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                var parameter = parameters[i];
                var parameterType = parameter.ParameterType;
                var getServiceMethod = getServiceMethodBase.MakeGenericMethod(parameterType);
                string key = null;
                injectArgs[i] = getServiceMethod.Invoke(this, new object[] { key });
            }

            injectMethod.Invoke(obj, injectArgs);
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
