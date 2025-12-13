using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PlaySound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume = 1f;

        public void Execute(GameObject mover, Collider2D other)
        {
            if (_clip == null) return;
            AudioSource.PlayClipAtPoint(_clip, transform.position, _volume);
        }
    }
}
