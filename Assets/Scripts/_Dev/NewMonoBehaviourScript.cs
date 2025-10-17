using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private List<Rigidbody2D> _rbs = new();

    private void Awake()
    {
        Debug.Log("NewMonoBehaviourScript Awake");
        StartupLifecycle.AddPostInjectListener(PostInject);
    }

    private void OnEnable()
    {
        Debug.Log("NewMonoBehaviourScript OnEnable");
    }

    void Start()
    {
        Debug.Log("NewMonoBehaviourScript Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            GetComponents<Rigidbody2D>(_rbs);
            Debug.Log($"Found {_rbs.Count} Rigidbody2D components on {gameObject.name}");
            //Debug.Log("V key was pressed");
            //StartupLifecycle.AddPostInjectListener(PostInject);
        }
    }

    private void PostInject()
    {
        Debug.Log("NewMonoBehaviourScript PostInject");
    }
}
