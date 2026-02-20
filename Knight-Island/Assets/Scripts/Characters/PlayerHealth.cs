using System;
using UnityEngine;

namespace Characters
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        public float CurrentHealth => _currentHealth;
        private float _currentHealth;
        
        public event Action<float, float>  OnHealthChanged;
        
        private void Start()
        {
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            _currentHealth += amount;
            
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }
    }
}

