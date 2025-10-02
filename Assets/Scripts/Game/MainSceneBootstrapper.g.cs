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
            Resolve(monoInjectObjects[0] as BreakoutGame.Paddle);
            Resolve(monoInjectObjects[1] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[2] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[3] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[4] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[5] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[6] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[7] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[8] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[9] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[10] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[11] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[12] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[13] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[14] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[15] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[16] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[17] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[18] as BreakoutGame.Brick);
            Resolve(monoInjectObjects[19] as BreakoutGame.BrickManager);
        }
        private void Resolve(BreakoutGame.Paddle monoInject)
        {
            string key = null;
            var arg0 = _compositionRoot.Resolve<BreakoutGame.IBallPaddleCollisionStrategy>(key);
            monoInject.Inject(arg0);
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
