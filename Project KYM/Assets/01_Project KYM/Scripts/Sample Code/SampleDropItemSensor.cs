using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDropItemSensor : MonoBehaviour
{
    public float sensorRadius = 3.0f; // 센서의 반지름
    public LayerMask dropItemLayer; // 센서가 감지할 레이어를 지정할 수 있는 변수
    public Collider[] overlappedDropItems; // 감지된 아이템의 콜라이더를 저장할 배열
    public int totalMoney = 0;
    public float totalHP = 0;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Gizmos의 색상을 빨간색으로 설정
        Gizmos.DrawWireSphere(transform.position, sensorRadius); // 현재 위치를 중심으로 반지름이 sensorRadius인 구체를 그립니다.
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
