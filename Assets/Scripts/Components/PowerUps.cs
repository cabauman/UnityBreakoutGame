using UnityEngine;

namespace BreakoutGame
{
    // Types: Timed,     Additive, Overridable
    //        Component, OneShot,  State
    public interface IPowerUp
    {
        void Execute(Collision2D collision);
    }

    public interface IPowerUpState
    {
        void Enter();
        void Exit();
    }

    public class TemporaryEffect : MonoBehaviour
    {
        public float Duration;
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration) Destroy(this);
        }
    }
}
