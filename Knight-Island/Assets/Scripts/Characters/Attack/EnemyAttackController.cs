using System.Collections.Generic;
using Characters.Movements;
using UnityEngine;

namespace Characters.Attack
{
    public class EnemyAttackController : CharacterAttackController
    {
        [SerializeField] private EnemyMovementController movementController;
        [SerializeField] private float attackDistance;
        [SerializeField] private Vector2 attackDistanceOffset;
        [SerializeField] private LayerMask attackFilter;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackDistanceOffset, attackDistance);
        }

        private void Update()
        {
            if (!movementController.Target) return;
            
            Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position + attackDistanceOffset, attackDistance, attackFilter);

            if (col == null) return;
            
            if (!IsAttacking && 
                col.transform == movementController.Target || col.transform.parent == movementController.Target)
            {
                Attack();
            }
        }
    }
}