using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private int damage = 999999;

        private void OnTriggerEnter(Collider other)
        {
            var hp = other.GetComponent<IHasHp>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}
