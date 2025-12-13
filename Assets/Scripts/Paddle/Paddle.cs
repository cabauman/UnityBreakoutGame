using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed class Paddle : MonoBehaviour
    {
        [SerializeField]
        private Ball _ballPresenter;
        [SerializeField]
        private Transform _initialBallPosTrfm;
        [SerializeField]
        private Transform _graphicTrfm;

        private void Start()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Mouse.current.leftButton.wasPressedThisFrame &&
                    EventSystem.current != null &&
                    !EventSystem.current.IsPointerOverGameObject())
                    //Mathf.Abs(_ballPresenter.Velocity.y) < Mathf.Epsilon)
                .Subscribe(_ => _ballPresenter.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(2f, 10f))
                .AddTo(this);
        }
    }
}
