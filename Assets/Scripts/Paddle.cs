using System;
using UniRx;
using UnityEngine;

[Serializable]
public class Paddle
{
    [SerializeField]
    private float _speed = 10;

    public Paddle()
    {
        ResetBallPos = new ReactiveCommand<Unit>();
    }

    public ReactiveCommand<Unit> ResetBallPos { get; }

    public float GetHorizontalTranslation(float normalizedValue)
    {
        return normalizedValue * _speed;
    }
}
