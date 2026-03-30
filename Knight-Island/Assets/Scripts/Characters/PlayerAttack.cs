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
        
        [Header("Système de Hitbox")]
        [SerializeField] private GameObject attackHitbox;
        [SerializeField] private float hitboxDistance = 0.5f;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        
        private PlayerMovementController _movementController; 
        private bool _isAttacking = false;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            _movementController = GetComponent<PlayerMovementController>();
            
            if (_animator == null) Debug.LogError("Animator introuvable !");
            if (_movementController == null) Debug.LogError("PlayerMovementController introuvable !");
        }

        private void Update()
        {
            if (_movementController.IsDead) return;

            if (Mouse.current.leftButton.wasPressedThisFrame && !_isAttacking)
            {
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            _isAttacking = true;
    
            _movementController.SetDead(true); 

            float lastX = _animator.GetFloat("InputX");
            float lastY = _animator.GetFloat("InputY");

            _animator.enabled = false;
            _spriteRenderer.flipX = false; // Reset du flip au début
    
            attackHitbox.SetActive(true);
            Vector2 hitboxPos = Vector2.zero;

            if (lastY > 0.1f) {
                _spriteRenderer.sprite = attackBackSprite;
                hitboxPos = Vector2.up;
            }
            else if (lastY < -0.1f) {
                _spriteRenderer.sprite = attackFrontSprite;
                hitboxPos = Vector2.down;
            }
            else if (lastX > 0.1f) {
                _spriteRenderer.sprite = attackRightSprite;
                hitboxPos = Vector2.right;
            }
            else if (lastX < -0.1f) {
                _spriteRenderer.sprite = attackRightSprite;
                _spriteRenderer.flipX = true;
                hitboxPos = Vector2.left;
            }

            attackHitbox.transform.localPosition = hitboxPos * hitboxDistance;
            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackDuration);
    
            attackHitbox.SetActive(false);
            _animator.enabled = true;
            _isAttacking = false;
    
            if (!_movementController.IsDead) 
            {
                _movementController.SetDead(false); 
            }
        }
    }
}