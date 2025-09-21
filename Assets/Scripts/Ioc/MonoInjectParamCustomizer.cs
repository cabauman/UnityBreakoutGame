using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    public sealed class MonoInjectParamCustomizer : MonoBehaviour
    {
        [SerializeField] private MonoInjectParam[] keys = new MonoInjectParam[0];

        public IReadOnlyList<MonoInjectParam> Keys => keys;
    }

    [Serializable]
    public sealed class MonoInjectParam
    {
        public string ParamName;
        public StringVariable Key;
    }
}
