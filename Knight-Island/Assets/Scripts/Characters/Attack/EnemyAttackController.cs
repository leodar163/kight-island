using Characters.Movements;
using UnityEngine;

namespace Characters.Attack
{
    public class EnemyAttackController : CharacterAttackController
    {
        [SerializeField] private EnemyMovementController movementController;
        [SerializeField] private float attackDistance;
        [SerializeField] private Vector2 attackDistanceOffset;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackDistanceOffset, attackDistance);
        }

        private void Update()
        {
            if (!movementController.Target) return;
            if (!IsAttacking &&
                Vector2.Distance(movementController.Target.position, 
                    attackDistanceOffset + (Vector2)transform.position) <= attackDistance)
            {
                Attack();
            }
        }
    }
}