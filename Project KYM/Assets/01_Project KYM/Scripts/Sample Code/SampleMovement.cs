using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMovement : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");    // horizontal   : A/D or Left/Right Arrow   => A키를 누르면 -1, D키를 누르면 1 값이다.
        float vertical = Input.GetAxis("Vertical");         // vertical     : W/S or Up/Down Arrow      => W키를 누르면 1, S키를 누르면 -1 값이다.
        
        // Debug.Log($"horizontal : {horizontal}, vertical : {vertical}");
        // Debug.Log($"DeltaTIme : {Time.deltaTime}");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.position += movement * speed * Time.deltaTime;
    }
}
