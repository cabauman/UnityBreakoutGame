using System;
using TMPro;
using GameCtor.FuseDI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutGame
{
    public sealed class PauseScreen : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _returnToTitleScreenButton;

        // [Header("Parameters")]
        // [SerializeField]
        // private float _delayBeforeDisplayingPlayAgainButton = 3f;
    }
}