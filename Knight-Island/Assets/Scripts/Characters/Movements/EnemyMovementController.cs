using System;
using UnityEngine;

namespace Characters.Movements
{
    [RequireComponent(typeof(CharacterMovement))]
    public class EnemyMovementController : MonoBehaviour
    {
        private CharacterMovement _charaMovement;
        [SerializeField] private Transform target;
        [Tooltip("La distance de la cible à partir de laquelle l'ennemie cesse d'accélerer")]
        [SerializeField] private float targetDeadZone = 0.5f;
        
        public Transform Target => target;
        
        private void OnValidate()
        {
            if(_charaMovement == null) TryGetComponent(out _charaMovement);
        }

        private void FixedUpdate()
        {
            Vector2 direction = Vector2.zero;

            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                direction = !target.gameObject.activeSelf || distance <= targetDeadZone
                    ?  direction
                    : (target.position - transform.position).normalized;

            } 
            
            _charaMovement.Direction = direction;
        }
    }
}