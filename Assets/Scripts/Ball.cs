using System;
using UniRx;
using UnityEngine;

[Serializable]
public class Ball
{
    [SerializeField]
    private float _initialForce = 50f;
    [SerializeField]
    private int _power = 1;

    public Ball()
    {
        Active = new ReactiveProperty<bool>(true);
    }

    public float InitialForce => _initialForce;

    public int Power => _power;

    public IReactiveProperty<bool> Active { get; private set; }
}
