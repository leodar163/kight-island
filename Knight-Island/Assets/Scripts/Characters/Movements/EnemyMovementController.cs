using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Movements
{
    [RequireComponent(typeof(CharacterMovement))]
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private Transform target;
        [Tooltip("La distance de la cible à partir de laquelle l'ennemie cesse d'accélerer")]
        [SerializeField] private float targetDeadZone = 0.5f;
        
        private Vector2 targetOffset =  Vector2.zero;
        
        public Vector2 TargetOffset => targetOffset;
        
        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private void Awake()
        {
            targetOffset = new Vector2
            {
                x = Random.Range(-0.3f, 0.3f),
                y = Random.Range(-0.3f, 0.3f),
            };
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
                float distance = Vector3.Distance(transform.position, target.position + (Vector3)targetOffset);
                direction = !target.gameObject.activeSelf || distance <= targetDeadZone
                    ?  direction
                    : ((target.position + (Vector3)targetOffset) - transform.position).normalized;
            } 
            
            charaMovement.Direction = direction;
        }
    }
}