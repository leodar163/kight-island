using System;
using System.Collections;
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
        private static readonly int Die = Animator.StringToHash("Die");

        [HideInInspector] [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private Health charaHealth;
        [Space] 
        [Min(1f/24f)][SerializeField] private float invincibleFrameBlinkDuration = 0.1f;
        
        private IEnumerator _invincibleFrameRoutine;
        
        private void OnValidate()
        {
            if (animator == null) TryGetComponent(out animator);
        }

        private void OnEnable()
        {
            charaHealth.onHealthReachZero.AddListener(TriggerDeath);
            charaHealth.onDamageTaken.AddListener(TriggerInvincibleFrame);
        }

        private void OnDisable()
        {
            charaHealth.onHealthReachZero.RemoveListener(TriggerDeath);
            charaHealth.onDamageTaken.RemoveListener(TriggerInvincibleFrame);
        }

        private void Update()
        {
            SetLastDirection();
        }

        private void SetLastDirection()
        {
            Vector2 facingDirection = charaMovement.FacingDirection;
            Vector2 direction = charaMovement.Direction;
            
            animator.SetFloat(Horizontal, facingDirection.x);
            animator.SetFloat(Vertical, facingDirection.y);
            animator.SetBool(Moving, direction.magnitude > 0.001f);
            
            spriteRenderer.flipX = facingDirection.x < -0.5f && Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y); 
        }

        private void TriggerDeath()
        {
            animator.SetTrigger(Die);
            StopInvincibleFrameRoutine();
        }

        private void TriggerInvincibleFrame()
        {
            StopInvincibleFrameRoutine();

            if (!charaHealth.IsInvincible) return;

            _invincibleFrameRoutine = InvincibleFrameRoutine();
            
            StartCoroutine(_invincibleFrameRoutine);
        }

        private void StopInvincibleFrameRoutine()
        {
            if (_invincibleFrameRoutine == null) return;
            StopCoroutine(_invincibleFrameRoutine);
            spriteRenderer.enabled = true;
        }
        
        private IEnumerator InvincibleFrameRoutine()
        {
            while (charaHealth.IsInvincible)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(invincibleFrameBlinkDuration);
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(invincibleFrameBlinkDuration);
            }
            spriteRenderer.enabled = true;
        }


    }
}
