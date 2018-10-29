using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BrickPresenter : MonoBehaviour
{
    [SerializeField]
    private int _initalHp = 1;
    [SerializeField]
    private PowerUpPresenter _powerUpPrefab;
    [SerializeField]
    private GameManager _gameManager;

    private void Start()
    {
        //Brick = new Brick(_initalHp);
        Brick.GameManager = _gameManager;

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

    //public Brick Brick { get; private set; }

    public Brick Brick { get; } = new Brick(1);

    private void InstantiatePowerUp(PowerUp powerUp)
    {
        var powerUpPresenter = Instantiate(_powerUpPrefab, transform.position, Quaternion.identity);
        powerUpPresenter.PowerUp = powerUp;
    }
}
