using UnityEngine;

namespace BreakoutGame
{
    public sealed class DisableOnCollide : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _layer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & _layer) == 0)
            {
                return;
            }

            collision.gameObject.SetActive(false);
        }
    }
}
