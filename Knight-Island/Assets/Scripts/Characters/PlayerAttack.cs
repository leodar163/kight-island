using System.Collections;
using Characters.Movements;
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
        private CharacterAnimator _charaAnimator;
        private CharacterMovement _charaMovement; 
        private CharacterHealth _health; 
        private bool _isAttacking = false;

        private void Awake()
        {
            _charaAnimator = GetComponentInChildren<CharacterAnimator>();            
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _charaMovement = GetComponent<CharacterMovement>();
            _health = GetComponent<CharacterHealth>(); 
            
            if (_charaAnimator == null) Debug.LogError("<color=red>MANQUANT : CharacterAnimator</color> sur " + gameObject.name);
            if (_animator == null) Debug.LogError("<color=red>MANQUANT : Animator</color> sur " + gameObject.name);
            if (_charaMovement == null) Debug.LogError("<color=red>MANQUANT : CharacterMovement</color> sur " + gameObject.name);
            if (_health == null) Debug.LogError("<color=red>MANQUANT : CharacterHealth</color> sur " + gameObject.name);
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if (Mouse.current.leftButton.wasPressedThisFrame && !_isAttacking)
            {
                PerformAttack();
            }
            
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                print("Clic droit détecté ! IsAttacking = " + _isAttacking);
                if (!_isAttacking) PerformAttack();
            }
        }

        private void PerformAttack()
        {
            _isAttacking = true;
            _charaMovement.CanMove = false; 
    
            _charaAnimator.PlayAttack();
    
            attackHitbox.SetActive(true);

            float x = _animator.GetFloat("Horizontal");
            float y = _animator.GetFloat("Vertical");
    
            Vector2 hitboxPos = (x == 0 && y == 0) ? Vector2.down : new Vector2(x, y).normalized;

            attackHitbox.transform.localPosition = hitboxPos * hitboxDistance;
    
            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackDuration);
    
            attackHitbox.SetActive(false);
            _isAttacking = false;
    
            if (!_health.IsDead) 
            {
                _charaMovement.CanMove = true; 
            }
        }
    }
}