using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItemCreateManager : MonoBehaviour
{
    public int spawnItemCount = 10; // ������ ������ ����
    public GameObject itemPrefab; // ������ ������
    public GameObject healingItemPrefab; // ȸ�� ������ ������
    public GameObject trapItemPrefab; // ���� ������ ������
    private void Start()
    {
        for(int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => ���� �����ϴ� Unity API �Լ�
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => ���� �����ϴ� Unity API �Լ�
            Instantiate(healingItemPrefab, randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < spawnItemCount; i++)
        {
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float y = 3f;

            Vector3 randomPosition = new Vector3(randomX, y, randomZ);

            // Instantiate => ���� �����ϴ� Unity API �Լ�
            Instantiate(trapItemPrefab, randomPosition, Quaternion.identity);
        }
    }

}
