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

        [HideInInspector] [SerializeField] private Animator animator;
        [HideInInspector] [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private CharacterHealth charaHealth;
        [Space]
        [SerializeField] private float invincibilityBlinkDuration = 0.1f;
        
        private IEnumerator invincibilityRoutine;
        
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
            SetLastDirection();
        }

        private void OnInvincible(bool isInvincible)
        {
            if (isInvincible)
            {
                if (invincibilityRoutine != null) return;

                invincibilityRoutine = InvincibilityRoutine();
                StartCoroutine(invincibilityRoutine);
            }
            else
            {
                if (invincibilityRoutine == null) return;
                StopCoroutine(invincibilityRoutine);
                invincibilityRoutine = null;
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

        private void SetLastDirection()
        {
            Vector2 direction = charaMovement.Direction;
            
            animator.SetFloat(Horizontal, direction.x);
            animator.SetFloat(Vertical, direction.y);
            animator.SetBool(Moving, direction.magnitude > 0);
        }
    }
}
