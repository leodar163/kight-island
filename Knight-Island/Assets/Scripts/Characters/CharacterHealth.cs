using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class CharacterHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float invincibilityDuration = 1.0f;
        [SerializeField] private float maxHealth = 100f;

        [Header("Events (Découplage)")]
        public UnityEvent onCharacterDied;

        public UnityEvent<bool> onInvicibleFrame;
        public event Action<float, float> OnHealthChanged;
        public event Action OnDamageTaken;
        public float CurrentHealth => _currentHealth; 
        
        private float _currentHealth;
        private bool _isInvincible;
        private bool _isDead;


        public bool IsDead => _isDead;

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

            onCharacterDied?.Invoke();
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
            onInvicibleFrame?.Invoke(_isInvincible);
            
            yield return new WaitForSeconds(invincibilityDuration);
            
            _isInvincible = false;
            onInvicibleFrame?.Invoke(_isInvincible);
        }
    }
}