using GameCtor.FuseDI;
using System;
using System.Collections.Generic;
using System.Linq;
using UniDig;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    public enum InjectMode
    {
        Single,
        Object,
        Recursive
    }

    public sealed class Injector
    {
        private BaseCompositionRoot _compositionRoot;
        private List<IMonoInject> _monoInjectList = new();

        public Injector(BaseCompositionRoot compositionRoot)
        {
            _compositionRoot = compositionRoot;
        }

        public T Instantiate<T>(T original, Vector3 position, InjectMode mode = InjectMode.Recursive)
            where T : UnityEngine.Component
        {
            var instance = UnityEngine.Object.Instantiate(original, position, Quaternion.identity);
            switch (mode)
            {
                case InjectMode.Single:
                    InjectSingle(instance.gameObject);
                    break;
                case InjectMode.Object:
                    InjectObject(instance.gameObject);
                    break;
                case InjectMode.Recursive:
                    InjectRecursive(instance.gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            return instance;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, InjectMode mode = InjectMode.Recursive)
        {
            var instance = UnityEngine.Object.Instantiate(original, position, Quaternion.identity);
            switch (mode)
            {
                case InjectMode.Single:
                    InjectSingle(instance);
                    break;
                case InjectMode.Object:
                    InjectObject(instance);
                    break;
                case InjectMode.Recursive:
                    InjectRecursive(instance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            return instance;
        }

        public void InjectSingle(GameObject go)
        {
            if (go.TryGetComponent<IMonoInject>(out var monoInject))
            {
                //monoInject.Accept(this);
            }
        }

        public void InjectObject(GameObject go)
        {
            _monoInjectList.Clear();
            go.GetComponents<IMonoInject>(_monoInjectList);
            foreach (var component in _monoInjectList)
            {
                //component.Accept(this);
            }
        }
        
        public void InjectRecursive(GameObject go)
        {
            _monoInjectList.Clear();
            go.GetComponentsInChildren<IMonoInject>(true, _monoInjectList);
            foreach (var component in _monoInjectList)
            {
                //component.Accept(this);
            }
        }

#if UNITY_EDITOR
        private List<MonoInjectParamCustomizer> _paramCustomizers = new();
        private Dictionary<string, string> _paramKeys = new();
        private Dictionary<string, string> GetParamKeys<T>(T monoInject)
            where T : Component
        {
            _paramCustomizers.Clear();
            monoInject.GetComponents<MonoInjectParamCustomizer>(_paramCustomizers);
            var paramCustomizer = _paramCustomizers
                .FirstOrDefault(x => x.MonoInjectComponent != null && x.MonoInjectComponent.GetType() == typeof(T));
            if (paramCustomizer != null)
            {
                _paramKeys.Clear();
                foreach (var key in paramCustomizer.Keys)
                {
                    if (key.Key != null && key.ParamName != null)
                    {
                        _paramKeys[key.ParamName] = key.Key.Value;
                    }
                }
            }
            return null;
        }
        
        public void Inject(BreakoutGame.Paddle monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IBallPaddleCollisionStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.GameOverScreen monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.ExtraBallPowerUpAction monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.BallManager>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.ProjectilePowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ProjectileCollisionStrategyDecorator>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.MagnetPowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.MagnetBounceStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.HeavyBallPowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.HeavyBallGameWorldEffect>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.TestMono monoInject)
        {
            string key = null;
            Dictionary<string, string> keys = GetParamKeys(monoInject);

            keys?.TryGetValue("myInt2", out key);
            var arg0 = _compositionRoot.Resolve<System.Int32>(key);
            keys?.TryGetValue("serviceA", out key);
            var arg1 = _compositionRoot.Resolve<BreakoutGame.ServiceA>(key);
            keys?.TryGetValue("powerUpSpawner", out key);
            var arg2 = _compositionRoot.Resolve<BreakoutGame.PowerUpSpawner>(key);
            keys?.TryGetValue("random", out key);
            var arg3 = _compositionRoot.Resolve<BreakoutGame.IRandom>(key);
            monoInject.Inject(arg0, arg1, arg2, arg3);
        }
        public void Inject(BreakoutGame.BrickManager monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IRandom>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.ExtraLifePowerUpAction monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.Hud monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.IncrementScoreCommand monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ScoreKeeper>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.ReverseBounceModifier monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ReverseBounceStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.SpawnPowerUpCommand monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IPowerUpSpawner>(key);
            monoInject.Inject(arg0);
        }
        public void Inject(BreakoutGame.Brick monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IPowerUpSpawner>(key);
            monoInject.Inject(arg0);
        }
        #endif
    }
}
