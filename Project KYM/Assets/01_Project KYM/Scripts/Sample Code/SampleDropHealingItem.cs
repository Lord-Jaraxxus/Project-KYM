using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropHealingItem : MonoBehaviour, SampleDropItemInterface
{
    public int healingValue = 100; // 회복 아이템의 회복량

    public void OnDropItemPicked(SampleDropItemSensor sensor)
    {
        sensor.totalHP += healingValue;
    }
}
