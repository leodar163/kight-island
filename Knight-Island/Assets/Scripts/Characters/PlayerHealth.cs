using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float invincibilityDuration = 1.0f;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private Image healthBarFill;
        
        public float CurrentHealth => _currentHealth;
        private float _currentHealth;
        private bool _isInvincible = false;
        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _playerMovement;
        
        public event Action<float, float>  OnHealthChanged;
        public event Action OnDamageTaken;
        
        private void Start()
        {
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerMovement = GetComponent<PlayerMovement>();
            OnHealthChanged += UpdateUI;
        }
        
        private void UpdateUI(float current, float max)
        {
            if (healthBarFill)
            {
                healthBarFill.fillAmount = current / max;
                print($"[HUD] Mise à jour visuelle : {healthBarFill.fillAmount * 100}%");
            }
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible) return;
            
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            
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
            if (_playerMovement)
            {
                _playerMovement.isDead = true;
                print("[HEALTH] Le joueur est mort, mouvements bloqués.");
            }
        }

        public void Heal(float amount)
        {
            _currentHealth += amount;
            
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        private IEnumerator InvincibilityRoutine()
        {
            _isInvincible = true;
            print("[HEALTH] Le personnage est invincible !");
            
            float timer = 0f;
            while (timer < invincibilityDuration)
            {
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
        
                yield return new WaitForSeconds(0.1f);
                timer += 0.1f;
            }

            _spriteRenderer.enabled = true;
            _isInvincible = false;
            print("[HEALTH] Invincibilité terminée.");
        }
        
        private void OnDestroy()
        {
            OnHealthChanged -= UpdateUI;
        }
    }
}

