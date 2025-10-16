using TMPro;
using GameCtor.FuseDI;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class Hud : MonoBehaviour
    {
        private const string NUM_LIVES_FMT = "Lives: {0}";

        [Inject] private Game _game;
        //[Inject] private ISubscriber<NumLivesChangedEvent> _numLivesChangedEventSubscriber;
        
        [SerializeField]
        private TextMeshProUGUI _numLivesLabel;

        private void Start()
        {
            //_game
            //    .NumLives
            //    .Subscribe(numLives => _numLivesLabel.text = string.Format(NUM_LIVES_FMT, numLives))
            //    .AddTo(this);
            // _numLivesChangedEventSubscriber.Subscribe()
            //     .Subscribe(numLives => _numLivesLabel.text = string.Format(NUM_LIVES_FMT, numLives));
        }
    }
}