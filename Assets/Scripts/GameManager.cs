using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private Ball _ball;
    [SerializeField]
    private Transform _leftWall;
    [SerializeField]
    private Transform _rightWall;
    [SerializeField]
    private Transform _topWall;

    private void Start()
    {
        _leftWall.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f));
        _rightWall.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f));
        _topWall.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f));
    }
}
