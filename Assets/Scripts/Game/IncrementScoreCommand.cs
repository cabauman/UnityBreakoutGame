using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class IncrementScoreCommand : MonoCommand
    {
        [SerializeField] private int _pointsToAdd = 100;
        [UniDig.Inject] private ScoreKeeper _scoreKeeper;

        public override void Execute()
        {
            _scoreKeeper.AddPoints(_pointsToAdd);
        }
    }
}