using UnityEngine.Search;

namespace BreakoutGame
{
    [System.Serializable]
    public class PowerUpConfig
    {
        [SearchContext("p: t:PowerUp", "asset")]
        public PowerUpPresenter Prefab;
        public int Weight;
    }
}