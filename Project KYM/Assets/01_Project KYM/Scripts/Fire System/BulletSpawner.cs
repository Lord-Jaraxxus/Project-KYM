using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // ÃÑ¾Ë ÇÁ¸®ÆÕ

    public void SpawnBullet()
    {
        if (bulletPrefab != null)
        {
            // GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation); // BulletSpawner À§Ä¡¿¡¼­ ÃÑ¾Ë »ý¼º
            Instantiate(bulletPrefab, transform.position, transform.rotation); // BulletSpawner À§Ä¡¿¡¼­ ÃÑ¾Ë »ý¼º
        }
    }
}
