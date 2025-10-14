using System;
using UnityEngine;
using UniDig;
using System.Reflection;
using System.Linq;

namespace GameCtor.DevToolbox
{
    [DefaultExecutionOrder(-5000)]
    public abstract class BaseCompositionRoot : MonoBehaviour, IInjector
    {
        [SerializeField]
        [Tooltip("Optional. Makes this composition root available to other composition roots to reference as a dependency." +
            "For example, Scene B might depend on this composition root in Scene A.")]
        private CompositionRootRuntimeItem _registrar;


        [SerializeField]
        [Tooltip("Optional. Other composition roots that this one depends on.")]
        private CompositionRootRuntimeItem[] _dependencies;

        protected virtual void Awake()
        {
            if (_registrar != null)
            {
                _registrar.Add(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_registrar != null)
            {
                _registrar.Remove(this);
            }

            Dispose();
        }

        public abstract void Dispose();

        public T Resolve<T>(string key)
        {
            var service = GetServiceShallow<T>(key);
            if (service == null)
            {
                //ULog.Debug($"{typeof(T).Name} is not registered in {this.name}. Checking dependent providers...");
                foreach (var compositionRoot in _dependencies)
                {
                    service = compositionRoot.GetService<T>(key);
                    if (service != null)
                    {
                        break;
                    }
                }
            }

            return service == null ? throw new Exception($"{typeof(T).Name} is not registered.") : service;
        }

        public T GetServiceShallow<T>(string key)
        {
            T service;
            if (key == null)
            {
                service = this is IServiceProvider<T> provider
                    ? provider.GetService()
                    : default;
            }
            else
            {
                service = this is INamedServiceProvider<T> namedProvider
                    ? namedProvider.GetService(key)
                    : default;
            }

            return service;
        }

        public void Inject(UnityEngine.Object obj)
        {
            var getServiceMethodBase = typeof(BaseCompositionRoot).GetMethod("Resolve");
            var type = obj.GetType();

            var injectMethods = type.GetMethods().Where(x => x.Name == "Inject").ToArray();
            if (injectMethods.Length == 0)
            {
                Debug.Log(type.Name);
                return;
            }

            var currentType = type;
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

            if (obj is Component comp)
            {
                foreach (var mono in comp.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (mono != obj) Inject(mono);
                }
            }
        }
    }

    public interface IInjector
    {
        void Inject(UnityEngine.Object obj);
    }

    public readonly struct TypeKey : IEquatable<TypeKey>
    {
        private readonly Type Type;
        private readonly string Key;

        public TypeKey(Type type, string key)
        {
            Type = type;
            Key = key;
        }

        public bool Equals(TypeKey other)
        {
            return Type == other.Type && Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Key);
        }

        public static bool operator ==(TypeKey left, TypeKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeKey left, TypeKey right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{Type.Name} ({Key})";
        }
    }
}
