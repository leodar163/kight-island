using System;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Moving = Animator.StringToHash("Moving");

        [HideInInspector] [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement playerMovement;
        private void OnValidate()
        {
            if (_animator == null) TryGetComponent(out _animator);
        }

        private void Update()
        {
            SetLastDirection();
        }

        private void SetLastDirection()
        {
            Vector2 direction = playerMovement.CurrentDirection;
            
            _animator.SetInteger(Horizontal, direction.x != 0 ? direction.x > 0 ? 1 : -1 : 0);
            _animator.SetInteger(Vertical, direction.y != 0 ? direction.y > 0 ? 1 : -1 : 0);
            _animator.SetBool(Moving, direction.magnitude > 0);
        }
    }
}
