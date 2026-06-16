using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Health : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float invincibilityDuration = 1.0f;
        [SerializeField] private float maxHealth = 100f;

        [Header("Events (Découplage)")]
        public UnityEvent onHealthReachZero;
        public UnityEvent onResurrect;
        public event Action<float, float> OnHealthChanged;
        public UnityEvent onDamageTaken;
        public float CurrentHealth => _currentHealth; 

        private float _currentHealth;
        private bool _isInvincible;
        private bool _isDead;


        public float MaxHealth => maxHealth;

        public bool IsDead => _isDead;
        public bool IsInvincible => _isInvincible;

        private void Start()
        {
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible || _isDead) return;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
            else if (invincibilityDuration > 0)
            {
                StartCoroutine(InvincibilityRoutine());
            }
            
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            onDamageTaken?.Invoke();
        }

        private void Die()
        {       
            if (_isDead) return;
            _isDead = true;

            StopAllCoroutines();

            onHealthReachZero?.Invoke();
        }

        public void Heal()
        {
            Heal(maxHealth);
        }
        
        public void Heal(float amount)
        {
            if (_isDead) return;
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void Resurrect()
        {
            if (!_isDead) return;
            _isDead =  false; 
            Heal();
            onResurrect.Invoke();
        }
        
        private IEnumerator InvincibilityRoutine()
        {
            _isInvincible = true;
            
            yield return new WaitForSeconds(invincibilityDuration);

            _isInvincible = false;
        }
    }
}