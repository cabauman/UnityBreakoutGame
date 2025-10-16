using GameCtor.FuseDI;
using System.Collections.Generic;
using GameCtor.FuseDI;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    public sealed class PrefabFactory1
    {
        private BaseCompositionRoot _compositionRoot;
        private List<IMonoInject> _monoInjectList = new();

        public PrefabFactory1(BaseCompositionRoot compositionRoot)
        {
            _compositionRoot = compositionRoot;
        }

        public T Create<T>(T prefab, Vector3 position, bool injectChildren = true)
            where T : UnityEngine.Object
        {
            var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            if (injectChildren)
            {
                _monoInjectList.Clear();
                if (instance is Component c)
                {
                    c.GetComponentsInChildren<IMonoInject>(true, _monoInjectList);
                }
                else if (instance is GameObject go)
                {
                    go.GetComponentsInChildren<IMonoInject>(true, _monoInjectList);
                }
                else
                {
                    return instance;
                }
                foreach (var component in _monoInjectList)
                {
                    //component.Accept(this);
                }
            }
            else
            {
                if (instance is Component c)
                {
                    if (c.TryGetComponent<IMonoInject>(out var monoInject))
                    {
                        //monoInject.Accept(this);
                    }
                }
                else if (instance is GameObject go)
                {
                    if (go.TryGetComponent<IMonoInject>(out var monoInject))
                    {
                        //monoInject.Accept(this);
                    }
                }
            }

            return instance;
        }

#if UNITY_EDITOR
        public void Visit(BreakoutGame.Paddle monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IBallPaddleCollisionStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.GameOverScreen monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.ExtraBallPowerUpAction monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.BallManager>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.ProjectilePowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ProjectileCollisionStrategyDecorator>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.MagnetPowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.MagnetBounceStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.HeavyBallPowerUp monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.HeavyBallGameWorldEffect>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.TestMono monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<System.Int32>(key);
            var arg1 = _compositionRoot.Resolve<BreakoutGame.ServiceA>(key);
            var arg2 = _compositionRoot.Resolve<BreakoutGame.PowerUpSpawner>(key);
            var arg3 = _compositionRoot.Resolve<BreakoutGame.IRandom>(key);
            monoInject.Inject(arg0, arg1, arg2, arg3);
        }
        public void Visit(BreakoutGame.BrickManager monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IRandom>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.ExtraLifePowerUpAction monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.Hud monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.Game>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.IncrementScoreCommand monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ScoreKeeper>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.ReverseBounceModifier monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.ReverseBounceStrategy>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.SpawnPowerUpCommand monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IPowerUpSpawner>(key);
            monoInject.Inject(arg0);
        }
        public void Visit(BreakoutGame.Brick monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IPowerUpSpawner>(key);
            monoInject.Inject(arg0);
        }
        #endif
    }
}
