using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Image healthSlider; 

        private void Awake()
        {
            if (!playerHealth || !healthSlider)
            {
                #if UNITY_EDITOR
                Debug.LogError($"[UI] Références manquantes sur {gameObject.name}.");
                #endif
                enabled = false;
                return;
            }

            playerHealth.OnHealthChanged += UpdateHealthBar;
        }

        private void Start()
        {
            UpdateHealthBar(playerHealth.CurrentHealth, 100f); 
        }

        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (maxHealth > 0)
            {
                healthSlider.fillAmount = currentHealth / maxHealth;
            }
            
        }

        private void OnDestroy()
        {
            if (playerHealth)
            {
                playerHealth.OnHealthChanged -= UpdateHealthBar;
            }
        }
    }
}