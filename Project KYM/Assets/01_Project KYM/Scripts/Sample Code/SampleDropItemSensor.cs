using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropItemSensor : MonoBehaviour
{
    public float sensorRadius = 3.0f; // ������ ������
    public LayerMask dropItemLayer; // ������ ������ ���̾ ������ �� �ִ� ����
    public Collider[] overlappedDropItems; // ������ �������� �ݶ��̴��� ������ �迭
    public int totalMoney = 0;
    public float totalHP = 0;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Gizmos�� ������ ���������� ����
        Gizmos.DrawWireSphere(transform.position, sensorRadius); // ���� ��ġ�� �߽����� �������� sensorRadius�� ��ü�� �׸��ϴ�.
    }

    private void Update()
    {
        overlappedDropItems = Physics.OverlapSphere(transform.position, sensorRadius, dropItemLayer);

        for(int i = 0; i < overlappedDropItems.Length; i++)
        {
            Debug.Log($" {overlappedDropItems[i].gameObject.name}");
        }
        
    }

    public void OverlapItemDestroy()
    {
        for (int i = 0; i < overlappedDropItems.Length; i++)
        {
            // SampleDropItem dropItem = overlappedDropItems[i].GetComponent<SampleDropItem>();
            // totalMoney += dropItem.moneyValue;

            SampleDropItemInterface dropItemInterface = overlappedDropItems[i].GetComponent<SampleDropItemInterface>();
            dropItemInterface.OnDropItemPicked(this);

            Destroy(overlappedDropItems[i].gameObject);
        }
        Debug.Log($"Current Total Money : {totalMoney}");
        Debug.Log($"Current HP : {totalHP}");
    }
}
