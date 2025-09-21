using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniDig;

namespace GameCtor.DevToolbox
{
    [DefaultExecutionOrder(-5000)]
    public abstract class BaseCompositionRoot : MonoBehaviour
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

        public T GetService<T>(string key, HashSet<TypeKey> resolving)
        {
            var service = GetServiceShallow<T>(key, resolving);
            if (service == null)
            {
                //ULog.Debug($"{typeof(T).Name} is not registered in {this.name}. Checking dependent providers...");
                foreach (var compositionRoot in _dependencies)
                {
                    service = compositionRoot.GetService<T>(key, resolving);
                    if (service != null)
                    {
                        break;
                    }
                }
            }

            return service == null ? throw new Exception($"{typeof(T).Name} is not registered.") : service;
        }

        public T GetServiceShallow<T>(string key, HashSet<TypeKey> resolving)
        {
            if (!resolving.Add(new(typeof(T), key)))
            {
                throw new Exception("Circular dependency detected: " + string.Join(" -> ", resolving.Append(new(typeof(T), key))));
            }

            var service = this is IServiceProvider<T> provider
                ? provider.GetService()
                : default;

            if (service == null)
            {
                service = this is INamedServiceProvider<T> namedProvider
                    ? namedProvider.GetService(key)
                    : default;
            }

            //if (typeof(T).IsGenericType)
            //{
            //    var service2 = this is IServiceProvider<object> provider2
            //        ? provider2.GetService()
            //        : default;
            //    if (service2 is T t)
            //    {
            //        service = t;
            //    }
            //}

            resolving.Remove(new(typeof(T), key));
            return service;
        }

        protected virtual void Dispose()
        {
        }
    }

    //public interface IServiceProvider<T>
    //{
    //    T GetService(string key, HashSet<TypeKey> resolving);
    //}

    //public interface IMonoInject
    //{
    //}

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
