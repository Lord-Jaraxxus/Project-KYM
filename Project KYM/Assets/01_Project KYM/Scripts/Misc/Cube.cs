using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public enum CubeType { Damage, Heal }

    public class Cube : MonoBehaviour
    {
        public CubeType cubeType; // Cube type (Damage or Heal)

        public float damage = 150f; // Damage amount
        public float heal = 500f; // Heal amount

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHasHp>(out var hpTarget) && cubeType == CubeType.Damage)
            {
                hpTarget.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (other.TryGetComponent<IHasHp>(out hpTarget) && cubeType == CubeType.Heal)
            {
                hpTarget.Heal(heal);
                Destroy(gameObject);
            }
        }

    }
}

