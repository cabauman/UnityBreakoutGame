using UnityEngine;

namespace BreakoutGame
{
    public interface ICollisionStrategy
    {
        void Resolve(Collision2D collision);
    }
}
