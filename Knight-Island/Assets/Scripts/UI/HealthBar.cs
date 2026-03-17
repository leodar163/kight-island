using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Image healthSlider; 

        private void Start()
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            
            UpdateHealthBar(playerHealth.CurrentHealth, 100f); 
        }

        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            float fillAmount = currentHealth / maxHealth;
            healthSlider.fillAmount = fillAmount;
            
            print($"[HUD] Mise à jour de la barre : {fillAmount * 100}%");
        }

        private void OnDestroy()
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
