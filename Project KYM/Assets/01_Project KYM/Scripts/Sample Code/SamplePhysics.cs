using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePhysics : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter : {other.name}");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log($"OnTriggerStay : {other.name}");
    }

    private void OnTriggerExit(Collider other) 
    {
        Debug.Log($"OnTriggerExit : {other.name}");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter !!! : {collision.collider.name}");
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"OnCollisionStay !!! : {collision.collider.name}");
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"OnCollisionExit !!! : {collision.collider.name}");
    }
}
