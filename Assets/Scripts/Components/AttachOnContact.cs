using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class AttachOnContact : MonoBehaviour
    {
        [SerializeField]
        private Transform _ballParent;

        private void Awake()
        {
            Ensure.NotNull(_ballParent);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.rigidbody.linearVelocity = Vector2.zero;
            collision.transform.SetParent(_ballParent);
            var pos = collision.transform.localPosition;
            pos.y = 0;
            collision.transform.localPosition = pos;
        }
    }
}
