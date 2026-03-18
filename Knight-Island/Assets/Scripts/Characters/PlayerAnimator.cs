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
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
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

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger(AttackTrigger);
            
            float lastX = _animator.GetFloat("InputX");
            float lastY = _animator.GetFloat("InputY");

            if (lastY > 0.1f) 
                _animator.Play("Knight_Attack_Back", 0, 0f);
            else if (lastY < -0.1f) 
                _animator.Play("Knight_Attack_Front", 0, 0f);
            else if (lastX > 0.1f) 
                _animator.Play("Knight_Attack_Right", 0, 0f);
            else if (lastX < -0.1f) 
                _animator.Play("Knight_Attack_Left", 0, 0f);
            else
                _animator.Play("Knight_Attack_Front", 0, 0f);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
