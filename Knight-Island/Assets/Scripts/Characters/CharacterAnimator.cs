using System;
using Characters.Movements;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Moving = Animator.StringToHash("Moving");

        [HideInInspector] [SerializeField] private Animator animator;
        [SerializeField] private CharacterMovement charaMovement;
        private void OnValidate()
        {
            if (animator == null) TryGetComponent(out animator);
        }

        private void Update()
        {
            SetLastDirection();
        }

        private void SetLastDirection()
        {
            Vector2 direction = charaMovement.Direction;
            
            animator.SetFloat(Horizontal, direction.x);
            animator.SetFloat(Vertical, direction.y);
            animator.SetBool(Moving, direction.magnitude > 0);
        }
    }
}
