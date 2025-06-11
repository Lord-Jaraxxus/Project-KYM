using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItemCreateManager : MonoBehaviour
{
    public int spawnItemCount = 10; // 생성할 아이템 개수
    public GameObject itemPrefab; // 아이템 프리팹
    public GameObject healingItemPrefab; // 회복 아이템 프리팹
    public GameObject trapItemPrefab; // 함정 아이템 프리팹
    private void Start()
    {
        for(int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => 복제 생성하는 Unity API 함수
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => 복제 생성하는 Unity API 함수
            Instantiate(healingItemPrefab, randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => 복제 생성하는 Unity API 함수
            Instantiate(trapItemPrefab, randomPosition, Quaternion.identity);
        }
    }

}
