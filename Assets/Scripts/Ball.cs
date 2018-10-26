using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float _initialForce = 50f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(_initialForce, _initialForce));
    }
}
