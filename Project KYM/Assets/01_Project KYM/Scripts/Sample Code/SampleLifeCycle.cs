using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLifeCycle : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Called Awake!");
    }

    void OnEnable()
    {
        Debug.Log("Called OnEnable!");
    }

    void Start()
    {
        Debug.Log("Called Start!");
    }

    void FixedUpdate()
    {
        Debug.Log("Called FixedUpdate!");
    }

    void Update()
    {
        Debug.Log("Called Update!");
    }

    void LateUpdate()
    {
        Debug.Log("Called LateUpdate!");
    }

    void OnDisable()
    {
        Debug.Log("Called OnDisable!");
    }

    void OnDestroy()
    {
        Debug.Log("Called OnDestroy!");
    }
}
