using GameCtor.DevToolbox;
using R3;
using TMPro;
using UnityEngine;

namespace BreakoutGame
{
    public class ActivePowerUpUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private PowerUpStateMachine _powerUpStateMachine;

        private void Start()
        {
            Ensure.NotNull(_label);
            Ensure.NotNull(_powerUpStateMachine);

            _powerUpStateMachine.CurrentState.Subscribe(state =>
            {
                if (state == null)
                {
                    _label.text = "Power-up: None";
                    return;
                }

                _label.text = $"Power-up: {state.Name}";
            });
        }
    }
}
