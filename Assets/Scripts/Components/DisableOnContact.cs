using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class DisableOnContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}
