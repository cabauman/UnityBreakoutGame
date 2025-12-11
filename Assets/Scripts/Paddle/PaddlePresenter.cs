using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddlePresenter : MonoBehaviour
    {
        [SerializeField]
        private BallPresenter _ballPresenter;
        [SerializeField]
        private Transform _initialBallPosTrfm;
        [SerializeField]
        private Transform _graphicTrfm;

        public void Init()
        {
            Paddle = new Paddle();

            Paddle
                .Width
                .Subscribe(xScale => _graphicTrfm.localScale = new Vector3(xScale, _graphicTrfm.localScale.y))
                .AddTo(this);

            Paddle
                .ResetBallPos
                .Subscribe(_ => ResetBallPos())
                .AddTo(this);
        }

        public Paddle Paddle { get; private set; }

        private void ResetBallPos()
        {
            _ballPresenter.transform.parent = transform;
            _ballPresenter.transform.position = _initialBallPosTrfm.position;
        }
    }
}
