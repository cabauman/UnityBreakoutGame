using UnityEngine.Search;

namespace BreakoutGame
{
    [System.Serializable]
    public class PowerUpConfig
    {
        [SearchContext("p: t:PowerUp", "asset")]
        public PowerUp Prefab;
        public int Weight;
    }
}