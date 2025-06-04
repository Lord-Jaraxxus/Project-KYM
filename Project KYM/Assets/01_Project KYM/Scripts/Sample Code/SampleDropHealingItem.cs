using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropHealingItem : MonoBehaviour, SampleDropItemInterface
{
    public int healingValue = 100; // ȸ�� �������� ȸ����

    public void OnDropItemPicked(SampleDropItemSensor sensor)
    {
        sensor.totalHP += healingValue;
    }
}
