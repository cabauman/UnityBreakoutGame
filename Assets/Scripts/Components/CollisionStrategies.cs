using System;
using UnityEngine;

namespace BreakoutGame
{
    public interface ICollisionStrategy
    {
        // Or Resolve
        void Execute(Collision2D collision);
    }
    public sealed class StickySurfaceStrategy : ICollisionStrategy
    {
        public void Execute(Collision2D collision)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class NullCollisionStrategy : ICollisionStrategy
    {
        public void Execute(Collision2D collision)
        {
        }
    }
}
