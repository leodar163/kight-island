using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Slider healthSlider; 

        private void Start()
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            
            UpdateHealthBar(playerHealth.CurrentHealth, 100f); // On peut ajuster le 100f plus tard
        }

        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            float fillAmount = currentHealth / maxHealth;
            healthSlider.value = fillAmount;
            
            print($"[HUD] Mise à jour de la barre : {fillAmount * 100}%");
        }

        private void OnDestroy()
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
