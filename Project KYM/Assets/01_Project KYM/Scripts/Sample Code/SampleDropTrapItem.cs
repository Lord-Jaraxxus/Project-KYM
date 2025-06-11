using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropTrapItem : MonoBehaviour, SampleDropItemInterface
{
    public int damageValue = 50; // ���� �������� ���ط�
    public void OnDropItemPicked(SampleDropItemSensor sensor)
    {
        sensor.totalHP -= damageValue; // ���� �������� ȹ���ϸ� HP ����
        Debug.Log("Trap item picked! Damage dealt: " + damageValue);
    }
}
