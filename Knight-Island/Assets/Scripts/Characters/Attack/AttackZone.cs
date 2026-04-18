using System;
using UnityEngine;

namespace Characters.Attack
{
    public class AttackZone : MonoBehaviour
    {
        [SerializeField] [Min(0)] private int damage;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}