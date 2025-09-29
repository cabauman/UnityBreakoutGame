using System.Collections.Generic;
using System.Linq;
using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    partial class MainSceneBootstrapper
    {
        #if !UNITY_EDITOR
        protected override void InjectSceneDependencies(MonoBehaviour[] behaviours)
        {
            //ULog.Debug("Injecting dependencies from generated source code.");
            Resolve(monoInjectObjects[0] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[1] as BreakoutGame.BrickManager);
        }
        private void Resolve(BreakoutGame.Brick monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IPowerUpSpawner>(key);
            monoInject.Inject(arg0);
        }
        private void Resolve(BreakoutGame.BrickManager monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IRandom>(key);
            monoInject.Inject(arg0);
        }
        #endif
    }
}
