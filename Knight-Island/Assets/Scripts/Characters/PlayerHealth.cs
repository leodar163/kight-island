using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float invincibilityDuration = 1.0f;
        [SerializeField] private float maxHealth = 100f;

        [Header("Events (Découplage)")]
        public UnityEvent onPlayerDied;
        public event Action<float, float> OnHealthChanged;
        public event Action OnDamageTaken;
        public float CurrentHealth => _currentHealth; 

        private float _currentHealth;
        private bool _isInvincible = false;
        private bool _isDead = false;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        public bool IsDead => _isDead;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible || _isDead) return;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            OnDamageTaken?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibilityRoutine());
            }
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;

            StopAllCoroutines();
            if (_spriteRenderer) _spriteRenderer.enabled = true;
            if (_animator) _animator.SetTrigger("Die");

            onPlayerDied?.Invoke();
        }

        public void Heal(float amount)
        {
            if (_isDead) return;
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        private IEnumerator InvincibilityRoutine()
        {
            _isInvincible = true;
            float timer = 0f;
            while (timer < invincibilityDuration)
            {
                if (_spriteRenderer) _spriteRenderer.enabled = !_spriteRenderer.enabled;
                yield return new WaitForSeconds(0.1f);
                timer += 0.1f;
            }
            if (_spriteRenderer) _spriteRenderer.enabled = true;
            _isInvincible = false;
        }
    }
}