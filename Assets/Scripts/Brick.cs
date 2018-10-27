using System;
using UniRx;
using UnityEngine;

[Serializable]
public class Brick : ISerializationCallbackReceiver
{
    [SerializeField]
    private int _initalHp = 1;

    public Brick()
    {
    }

    public IReactiveProperty<int> Hp { get; private set; }

    public IReadOnlyReactiveProperty<bool> Active { get; private set; }

    public IReactiveCommand<Ball> DecreaseHp { get; private set; }

    public IReactiveCommand<Unit> ResetHp { get; private set; }

    public void OnAfterDeserialize()
    {
        Hp = new ReactiveProperty<int>(_initalHp);

        Active = Hp.Select(x => x > 0).ToReactiveProperty();

        DecreaseHp = Hp.Select(x => x > 0).ToReactiveCommand<Ball>();
        DecreaseHp.Subscribe(ball => Hp.Value -= ball.Power);

        ResetHp = new ReactiveCommand<Unit>();
        ResetHp.Subscribe(_ => Hp.Value = _initalHp);
    }

    public void OnBeforeSerialize()
    {
    }
}
