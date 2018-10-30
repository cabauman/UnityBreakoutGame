using UniRx;
using UnityEngine;

public class PaddlePresenter : MonoBehaviour
{
    [SerializeField]
    private BallPresenter _ballPresenter;
    [SerializeField]
    private Transform _initialBallPosTrfm;
    [SerializeField]
    private Transform _graphicTrfm;

    public void Init()
    {
        Paddle = new Paddle();

        Observable
            .EveryUpdate()
            .Select(_ => Input.mousePosition)
            .Subscribe(UpdateXPosition)
            .AddTo(this);

        Paddle
            .Width
            .Subscribe(xScale => _graphicTrfm.localScale = new Vector3(xScale, _graphicTrfm.localScale.y))
            .AddTo(this);

        Paddle
            .ResetBallPos
            .Subscribe(_ => ResetBallPos())
            .AddTo(this);
    }

    public Paddle Paddle { get; private set; }

    private void UpdateXPosition(Vector3 mousePos)
    {
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        var xPos = Camera.main.ScreenToWorldPoint(mousePos).x;
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    private void ResetBallPos()
    {
        _ballPresenter.transform.parent = transform;
        _ballPresenter.transform.position = _initialBallPosTrfm.position;
    }
}
