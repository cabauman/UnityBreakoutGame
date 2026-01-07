using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace BreakoutGame
{
    [Serializable]
    public class LevelData
    {
        public int version = 1;
        public int level;
        public List<BrickData> bricks = new();
    }

    [Serializable]
    public class BrickData
    {
        // Addressables key, e.g. "brick.basic.red"
        public string type;
        public AssetReferenceGameObject prefab;
        public int x;
        public int y;
    }
}
