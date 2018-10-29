using UniRx;

public class Ball
{
    public Ball(float initialForce, int power)
    {
        InitialForce = initialForce;
        Power = power;
        Active = new ReactiveProperty<bool>(true);
    }

    public float InitialForce { get; }

    public int Power { get; }

    public IReactiveProperty<bool> Active { get; }
}
