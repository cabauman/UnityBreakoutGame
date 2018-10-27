using UniRx;
using UnityEngine;

public class PaddlePresenter : MonoBehaviour
{
    [SerializeField]
    private Paddle _paddle;
    [SerializeField]
    private BallPresenter _ballPresenter;
    [SerializeField]
    private Transform _initialBallPosTrfm;

    private void Start()
    {
        Observable
            .EveryUpdate()
            //.Select(_ => Input.GetAxis("Horizontal"))
            //.Select(_paddle.GetHorizontalTranslation)
            //.Subscribe(MoveHorizontally)
            .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
            .Subscribe(UpdateXPosition)
            .AddTo(this);

        _paddle
            .ResetBallPos
            .Subscribe(
                _ =>
                {
                    _ballPresenter.transform.parent = transform;
                    _ballPresenter.transform.position = _initialBallPosTrfm.position;
                })
            .AddTo(this);
    }

    public Paddle Paddle => _paddle;

    public void MoveHorizontally(float translation)
    {
        translation *= Time.deltaTime;
        transform.Translate(new Vector3(translation, 0, 0));
    }

    private void UpdateXPosition(float xPos)
    {
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}
