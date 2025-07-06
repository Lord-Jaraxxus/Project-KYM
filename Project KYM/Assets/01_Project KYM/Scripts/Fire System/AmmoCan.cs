using KYM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class AmmoCan : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<Bullet>(out var bullet))
            {
                bullet.shooter.AddReserveAmmo(30); // 30¹ß Ãß°¡
                Destroy(gameObject);
            }
        }

    }
}

