using System;
using UnityEngine;

namespace Characters.Movements
{
    [RequireComponent(typeof(CharacterMovement))]
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private Transform target;
        [Tooltip("La distance de la cible à partir de laquelle l'ennemie cesse d'accélerer")]
        [SerializeField] private float targetDeadZone = 0.5f;
        
        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private void OnValidate()
        {
            if(charaMovement == null) TryGetComponent(out charaMovement);
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
            
            charaMovement.Direction = direction;
        }
    }
}