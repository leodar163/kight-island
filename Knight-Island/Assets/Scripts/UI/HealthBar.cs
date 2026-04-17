using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private CharacterHealth characterHealth;
        [SerializeField] private Image healthSlider; 

        private void Awake()
        {
            if (!characterHealth || !healthSlider)
            {
                #if UNITY_EDITOR
                Debug.LogError($"[UI] Références manquantes sur {gameObject.name}.");
                #endif
                enabled = false;
                return;
            }

            characterHealth.OnHealthChanged += UpdateHealthBar;
        }

        private void Start()
        {
            UpdateHealthBar(characterHealth.CurrentHealth, 100f); 
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
            if (characterHealth)
            {
                characterHealth.OnHealthChanged -= UpdateHealthBar;
            }
        }
    }
}