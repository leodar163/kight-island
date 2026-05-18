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
        private readonly List<Collider2D> _colResults = new();

        private void OnValidate()
        {
            if (col == null) TryGetComponent(out col);
        } 

        private void OnEnable()
        {
            ContactFilter2D filter = new ()
            {
                layerMask = targetLayers,
                useLayerMask = true
            };

            col.Overlap(filter, _colResults);

            foreach (Collider2D result in _colResults)
            {
                print(result.gameObject.layer);
                if (result.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
            }
            
            _colResults.Clear();
        }
    }
}