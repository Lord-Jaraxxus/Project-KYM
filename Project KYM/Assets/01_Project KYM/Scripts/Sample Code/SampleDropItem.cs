using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropItem : MonoBehaviour, SampleDropItemInterface
{
    public int moneyValue;

    public void OnDropItemPicked(SampleDropItemSensor sensor) 
    {
        sensor.totalMoney += moneyValue;
    }

    private void Awake()
    {
        moneyValue = Random.Range(100, 1000);
    }
}
