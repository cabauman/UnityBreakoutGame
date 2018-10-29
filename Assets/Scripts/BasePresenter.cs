using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BasePresenter<T> : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    //public static BasePresenter<T> Create(T model)
    //{
    //    var presenter = Instantiate<BasePresenter<T>>(_prefab);
    //    presenter.Model = model;
    //}

    public T Model { get; set; }
}
