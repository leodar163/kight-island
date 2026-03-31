using System;
using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
        [Header("Références")]
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private CharacterHealth charaHealth;
        [Space]
        [SerializeField] private float invincibilityBlinkDuration = 0.5f;
        
        private PlayerMovementController _playerController;
        private IEnumerator _invincibilityRoutine;
        
        private void Awake()
        {
            _playerController = GetComponent<PlayerMovementController>();
        }
        
        private void OnValidate()
        {
            if (animator == null) TryGetComponent(out animator);
            if (spriteRenderer == null) TryGetComponent(out spriteRenderer);
        }

        private void OnEnable()
        {
            charaHealth.onCharacterDied.AddListener(SetTriggerDied);
            charaHealth.onInvicibleFrame.AddListener(OnInvincible);
        }

        private void OnDisable()
        {
            charaHealth.onCharacterDied.RemoveListener(SetTriggerDied);
            charaHealth.onInvicibleFrame.RemoveListener(OnInvincible);
        }

        private void Update()
        {
            if (charaHealth != null && charaHealth.IsDead) return;
            HandleAnimations();
        }
        
        private void HandleAnimations()
        {
            Vector2 direction = charaMovement.Direction;
    
            if (direction.sqrMagnitude < 0.01f && _playerController != null)
            {
                direction = _playerController.CurrentDirection;
            }

            if (direction.sqrMagnitude > 0.01f)
            {
                animator.SetFloat(Horizontal, direction.x);
                animator.SetFloat(Vertical, direction.y);
            }
    
            animator.SetBool(Moving, charaMovement.Direction.sqrMagnitude > 0.01f);
        }
        
        public void PlayAttack()
        {
            if (animator) animator.SetTrigger(AttackTrigger);
        }

        private void OnInvincible(bool isInvincible)
        {
            if (isInvincible)
            {
                if (_invincibilityRoutine != null) return;

                _invincibilityRoutine = InvincibilityRoutine();
                StartCoroutine(_invincibilityRoutine);
            }
            else
            {
                if (_invincibilityRoutine == null) return;
                StopCoroutine(_invincibilityRoutine);
                _invincibilityRoutine = null;
                spriteRenderer.enabled = true;
            }
        }

        private IEnumerator InvincibilityRoutine()
        {
            while (true)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(invincibilityBlinkDuration);
            }
        }
        
        private void SetTriggerDied()
        {
            if (animator) animator.SetTrigger(Die);
        }
    }
}
