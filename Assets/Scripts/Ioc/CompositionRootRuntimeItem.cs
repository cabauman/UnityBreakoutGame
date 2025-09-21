using System.Collections.Generic;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    [CreateAssetMenu(fileName = "CompositionRootRuntimeItem", menuName = "DevToolbox/IoC/CompositionRootRuntimeItem")]
    public class CompositionRootRuntimeItem : ScriptableObject
    {
        private BaseCompositionRoot _instance;

        public void Add(BaseCompositionRoot instance)
        {
            _instance = instance;
        }

        public void Remove(BaseCompositionRoot instance)
        {
            if (instance != _instance)
            {
                //ULog.Error("Composition root not registered.");
                return;
            }
            _instance = null;
        }

        public T GetService<T>(string key, HashSet<TypeKey> resolving)
        {
            if (_instance == null)
            {
                //ULog.Error("Composition root not registered.");
                return default;
            }

            var service = _instance.GetServiceShallow<T>(key, resolving);
            return service;
        }
    }
}
