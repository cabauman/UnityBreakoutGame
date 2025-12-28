using System;
using UnityEngine;

namespace BreakoutGame
{
    public interface ICollisionStrategy
    {
        void Resolve(Collision2D collision);
    }

    public sealed class StickySurfaceStrategy : ICollisionStrategy
    {
        public void Resolve(Collision2D collision)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class NullCollisionStrategy : ICollisionStrategy
    {
        public void Resolve(Collision2D collision)
        {
        }
    }
}
