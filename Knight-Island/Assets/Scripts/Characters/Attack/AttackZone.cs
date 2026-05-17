using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Attack
{
    [RequireComponent(typeof(Collider2D))]
    public class AttackZone : MonoBehaviour
    {
        [SerializeField] [Min(0)] private int damage;
        [HideInInspector] [SerializeField] private Collider2D col;
        [SerializeField] private LayerMask targetLayers;
        private readonly List<Collider2D> colResults = new();

        private void OnValidate()
        {
            if (col == null) TryGetComponent(out col);
        } 

        private void OnEnable()
        {
            ContactFilter2D filter = new ()
            {
                layerMask = targetLayers
            };

            col.Overlap(filter, colResults);

            foreach (Collider2D result in colResults)
            {
                if (result.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
            }
            
            colResults.Clear();
        }
    }
}