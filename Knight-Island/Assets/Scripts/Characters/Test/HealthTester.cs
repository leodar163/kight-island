using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Test
{
    public class HealthTester : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;

        private void Awake()
        {
            if (!playerHealth) 
            {
                Debug.LogWarning($"[TEST] PlayerHealth manquant sur {gameObject.name}. Script désactivé.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                playerHealth.TakeDamage(50f);
                print($"[TEST] Dégâts appliqués. Vie : {playerHealth.CurrentHealth}");
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                playerHealth.Heal(10f);
                print($"[TEST] Soin appliqué. Vie : {playerHealth.CurrentHealth}");
            }
        }
    }
}