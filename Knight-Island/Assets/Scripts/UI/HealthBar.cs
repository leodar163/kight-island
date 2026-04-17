using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image healthSlider; 

        private void Awake()
        {
            if (!health || !healthSlider)
            {
                #if UNITY_EDITOR
                Debug.LogError($"[UI] Références manquantes sur {gameObject.name}.");
                #endif
                enabled = false;
                return;
            }

            health.OnHealthChanged += UpdateHealthBar;
        }

        private void Start()
        {
            UpdateHealthBar(health.CurrentHealth, 100f); 
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
            if (health)
            {
                health.OnHealthChanged -= UpdateHealthBar;
            }
        }
    }
}