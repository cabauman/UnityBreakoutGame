using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PlayAudioClipCommand : MonoCommand
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] private AudioSource _audioSource;

        public override void Execute()
        {
            _audioSource.PlayOneShot(_clip, _volume);
        }
    }
}