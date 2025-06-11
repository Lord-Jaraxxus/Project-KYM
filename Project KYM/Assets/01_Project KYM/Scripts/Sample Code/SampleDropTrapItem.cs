using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropTrapItem : MonoBehaviour, SampleDropItemInterface
{
    public int damageValue = 50; // 함정 아이템의 피해량
    public void OnDropItemPicked(SampleDropItemSensor sensor)
    {
        sensor.totalHP -= damageValue; // 함정 아이템을 획득하면 HP 감소
        Debug.Log("Trap item picked! Damage dealt: " + damageValue);
    }
}
