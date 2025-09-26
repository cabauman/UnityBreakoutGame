using GameCtor.DevToolbox;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("NewMonoBehaviourScript Awake");
        StartupLifecycle.Initialize();
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
            Debug.Log("V key was pressed");
            StartupLifecycle.AddPostInjectListener(PostInject);
        }
    }

    private void PostInject()
    {
        Debug.Log("NewMonoBehaviourScript PostInject");
    }
}
