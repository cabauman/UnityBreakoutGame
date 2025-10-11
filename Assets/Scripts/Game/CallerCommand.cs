using UnityEngine;
using UnityEngine.Events;

namespace BreakoutGame
{
    public sealed class CallerCommand : MonoCommand
    {
        [SerializeField] private UnityEvent _action;

        public override void Execute()
        {
            _action?.Invoke();
        }
    }
}