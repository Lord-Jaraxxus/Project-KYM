using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;

public class SampleCharacter : MonoBehaviour
{
    int hp;
    int maxHp;
    int money;
    float timer;
    bool isPoison;

    void Awake()
    {
        maxHp = 100;
        hp = 100;
        money = 0;
        timer = 0;
        isPoison = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Red Cube") { hp -= 30; }
        else if (other.gameObject.name == "Blue Cube") { hp = maxHp; }
        else if (other.gameObject.name == "Green Cube") { isPoison = true; }

        UnityEngine.Debug.Log($"HP : {hp}");
    }

    void OnTriggerStay(Collider other)
    {
        if (isPoison)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 0.5f)
            {
                hp -= 10;
                UnityEngine.Debug.Log($"HP : {hp}");
                timer = 0;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Green Cube") 
        { 
            isPoison = false; 
            timer = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ITEM_BOX"))
        {
            Destroy(collision.gameObject);
            money += 1000;
            UnityEngine.Debug.Log($"Money : {money}");
        }
    }

}
