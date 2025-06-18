using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // �Ѿ� ������

    public void SpawnBullet()
    {
        if (bulletPrefab != null)
        {
            // GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation); // BulletSpawner ��ġ���� �Ѿ� ����
            Instantiate(bulletPrefab, transform.position, transform.rotation); // BulletSpawner ��ġ���� �Ѿ� ����
        }
    }
}
