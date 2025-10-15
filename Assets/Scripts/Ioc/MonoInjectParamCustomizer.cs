using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    public sealed class MonoInjectParamCustomizer : MonoBehaviour
    {
        [SerializeField] private MonoInjectParam[] keys = new MonoInjectParam[0];
        [SerializeField] private MonoBehaviour monoInjectComponent;

        public IReadOnlyList<MonoInjectParam> Keys => keys;

        // Serialized reference to the IMonoInject component on this GameObject.
        // Stored here so the editor persists the user's choice.
        public MonoBehaviour MonoInjectComponent
        {
            get => monoInjectComponent;
            set => monoInjectComponent = value;
        }
    }

    [Serializable]
    public sealed class MonoInjectParam
    {
        public string ParamName;
        public StringVariable Key;
    }
}
