using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Health health;

        private void Awake()
        {
            if (!healthBarFill || !health)
            {
                #if UNITY_EDITOR
                Debug.LogError($"[UI] Références manquantes sur {gameObject.name}. Script désactivé.");
                #endif
                enabled = false;
                return;
            }

            health.OnHealthChanged += UpdateUI;
        }

        private void UpdateUI(float current, float max)
        {
            if (max > 0)
            {
                healthBarFill.fillAmount = current / max;
            }
        }

        private void OnDestroy()
        {
            if (health)
            {
                health.OnHealthChanged -= UpdateUI;
            }
        }
    }
}