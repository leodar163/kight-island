using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Sprites d'Attaque")]
        [SerializeField] private Sprite attackFrontSprite;
        [SerializeField] private Sprite attackBackSprite;
        [SerializeField] private Sprite attackRightSprite;
        [SerializeField] private Sprite attackLeftSprite;

        [Header("Réglages")]
        [SerializeField] private float attackDuration = 0.3f;
        
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _playerMovement;
        private bool _isAttacking = false;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            if (_animator == null) Debug.LogError("Animator introuvable sur l'enfant !");
        }

        private void Update()
        {
            if (_playerMovement.isDead) return;

            if (Mouse.current.leftButton.wasPressedThisFrame && !_isAttacking)
            {
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            _isAttacking = true;
            _playerMovement.isAttacking = true;

            float lastX = _animator.GetFloat("InputX");
            float lastY = _animator.GetFloat("InputY");

            _animator.enabled = false;
            
            _spriteRenderer.flipX = false;

            if (lastY > 0.1f)
            {
                _spriteRenderer.sprite = attackBackSprite;
            }
            else if (lastY < -0.1f)
            {
                _spriteRenderer.sprite = attackFrontSprite;
            }
            else if (lastX > 0.1f)
            {
                _spriteRenderer.sprite = attackRightSprite;
            }
            else if (lastX < -0.1f)
            {
                _spriteRenderer.sprite = attackRightSprite; 
        
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.sprite = attackFrontSprite;
            }

            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackDuration);
            
            _animator.enabled = true;
            _isAttacking = false;
            _playerMovement.isAttacking = false;
        }
    }
}