using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BrickPresenter : MonoBehaviour
{
    [SerializeField]
    private int _initialHp = 1;
    [SerializeField]
    [Range(0, 10)]
    private int _powerUpSpawnOdds = 3;
    [SerializeField]
    private PowerUpPresenter _powerUpPrefab;

    public void Init()
    {
        Brick = new Brick(_initialHp, _powerUpSpawnOdds);

        this
            .OnCollisionEnter2DAsObservable()
            .Select(collision => collision.collider.GetComponent<BallPresenter>().Ball)
            .Subscribe(ball => Brick.RespondToBallCollision.Execute(ball))
            .AddTo(this);

        Brick
            .PowerUpCreated
            .Subscribe(InstantiatePowerUp)
            .AddTo(this);

        Brick
            .Active
            .Subscribe(value => gameObject.SetActive(value))
            .AddTo(this);
    }

    public Brick Brick { get; private set; }

    private void InstantiatePowerUp(PowerUp powerUp)
    {
        var powerUpPresenter = Instantiate(_powerUpPrefab, transform.position, Quaternion.identity);
        powerUpPresenter.PowerUp = powerUp;
    }
}
